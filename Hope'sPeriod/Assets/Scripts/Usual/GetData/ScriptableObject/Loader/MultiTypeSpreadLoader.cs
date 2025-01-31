using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Net.Http;
using Newtonsoft.Json;
using System.Reflection;
using SpreadInfo;
using VInspector.Libs;
using Object = System.Object;

[CreateAssetMenu(menuName = "LoadData/Loader/MultiTypeSpreadSheet")]
public class SpreadSheetListLoader: RawListDataLoader {
    
    [SerializeField] private string m_SpreadSheetID;
    [SerializeField] private string m_ApiKey;
    [SerializeField] private int splitRange = 1;
    
    private class SpreadSheetType {
    
        public string range;
        public string majorDimension;
        public List<List<string>> values;
    }
        
    public override List<List<List<string>>> Load(string path) {

        HttpClient client = new();
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{m_SpreadSheetID}/values/{path}?key={m_ApiKey}";
        var data = client.GetStringAsync(url).Result;
        var list = JsonConvert.DeserializeObject<SpreadSheetType>(data).values;
        
        List<List<List<string>>> fixList = new();
        for (int i = 0; i < list[0].Count; i++) {

            if (i % splitRange == 0) {
                fixList.Add(new List<List<string>>());
                foreach (var factor in list) {
                    fixList[^1].Add(new List<string>());
                }
            }
            
            for (int j = 0; j < list.Count; j++) {

                if (i >= list[j].Count)
                    break;
                fixList[^1][j].Add(list[j][i]);
            }
        }

        return fixList;
    }
};

[CreateAssetMenu(menuName = "LoadData/MultiTypeLoader")]
public class MultiTypeParser : DataParserBase {

    [SerializeField] private RawListDataLoader m_DataLoader;
    
    public void Generate(string path) {

        //check path is correct
        if (new[] {m_NameSpace, path}.Any(string.IsNullOrEmpty)) {
            throw new NullReferenceException("NameSpace and Path must not null or empty");
        }
        
        var rawDatas = m_DataLoader.Load(path);

        List<TypeAndName> header;
        List<List<string>> data;
        int useDictionary = m_DataLoader.UsingFirstRawToKey ? 1 : 0; 
        (header, data) =  Divide(rawDatas[0].Skip(useDictionary).ToList());
        string dataTypeName = DataTypeName(path);

        Type dataType = Type.GetType($"{m_NameSpace}.{dataTypeName}");
        if (dataType is null) {
            
            GenerateDataType(dataTypeName, header, true);
            AssetDatabase.Refresh();
        }
        else {
            Debug.Log($"already exist {dataTypeName} Type");
        }
    }

    public void Sync(string path) {
        if (new[] {m_NameSpace, path}.Any(string.IsNullOrEmpty)) {
            throw new NullReferenceException("NameSpace and Path must not null or empty");
        }
                
        var rawDatas = m_DataLoader.Load(path);
        
        List<TypeAndName> header;
        List<List<string>> data;
        string dataTypeName = DataTypeName(path);
        
        Type dataType = Type.GetType($"{m_NameSpace}.{dataTypeName}");
        if (dataType is null) {
                    
            throw new NullReferenceException("Type isn't exist. please press Generate button");
            return;
        }
        else {
            SyncData(rawDatas, dataTypeName);
        }
    }

    private void SyncData(List<List<List<string>>> data, string dataTypeName) {
        
        Type dataType = Type.GetType($"{m_NameSpace}.{dataTypeName}");
        Type dataTableType = Type.GetType($"{m_NameSpace}.{dataTypeName}Table");
        
        string directoryPath = $@"Assets\{m_NameSpace}\Generated\{dataTypeName}Table.asset";
        UnityEngine.Object targetTable = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(directoryPath);
        
        if (targetTable is null) {
            targetTable = CreateInstance(dataTableType);
        
            AssetDatabase.CreateAsset(targetTable, directoryPath);
            AssetDatabase.Refresh();
        }
        
        List<(int, Object)> keyAndValue = new();

        int index = 0;
        bool useDictionary = m_DataLoader.UsingFirstRawToKey;
        foreach (var factor in data) {
        
            List<TypeAndName> header;
            List<List<string>> rawDatas;
            (header, rawDatas) =  Divide(factor.Skip(useDictionary ? 1 : 0).ToList());
            
            
            int code = index++;
            if(useDictionary)
                code = int.Parse(factor[0][0]);
            
            FieldInfo[] fields = dataType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            List<Object> list = new();
            foreach (var rawData in rawDatas) {

                if (rawData.Count == 0) break;
                
                if (rawData[0].IsNullOrEmpty()) break;
                
                Object row = Activator.CreateInstance(dataType);
                for (int i = 0; i < fields.Length; i++) {

                    Type fieldType = fields[i].FieldType;
                    Object value = base.Parse(fieldType, rawData[i]);
                    fields[i].SetValue(row, value);    
                }

                list.Add(row);
            }

            var fixList = ConvertList(list, dataType);
            keyAndValue.Add((code, fixList));
        }
        
        FieldInfo dataTable = dataTableType.GetField("m_DataTable", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo addFunc = dataTable.FieldType.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
        MethodInfo clearFunc = dataTable.FieldType.GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance);
        Object target = dataTable.GetValue(targetTable);
        
        clearFunc.Invoke(target, new object[] {});
        
        
        foreach (var context in keyAndValue) {

            addFunc.Invoke(target, new object[] {context.Item1, context.Item2});
        }
        
        //save on disk
        EditorUtility.SetDirty(targetTable);
        Debug.Log("Load complete");
    }

    public static IList ConvertList(List<Object> rawList, Type targetType) {

        MethodInfo casting = typeof(Enumerable).GetMethod("Cast", BindingFlags.Public | BindingFlags.Static)!
            .MakeGenericMethod(targetType);

        var result = (IEnumerable)casting.Invoke(null, new object[] { rawList });

        MethodInfo toList = typeof(Enumerable).GetMethod("ToList", BindingFlags.Public | BindingFlags.Static)!
            .MakeGenericMethod(targetType);
        return (IList)toList.Invoke(null, new Object[] { result });
    }
}