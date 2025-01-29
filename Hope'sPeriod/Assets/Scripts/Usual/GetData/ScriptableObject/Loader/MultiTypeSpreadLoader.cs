using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Net.Http;
using Newtonsoft.Json;


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
        
        var rawData = m_DataLoader.Load(path);

        List<TypeAndName> header;
        List<List<string>> data;
        int useDictionary = m_DataLoader.UsingFirstRawToKey ? 1 : 0; 
        (header, data) =  Divide(rawData[0].Skip(useDictionary).ToList());
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
}