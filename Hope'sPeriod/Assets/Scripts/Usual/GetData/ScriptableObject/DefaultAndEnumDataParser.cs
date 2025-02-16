#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;
using UnityEngine.Serialization;
using Object = System.Object;

[CreateAssetMenu(menuName = "LoadData/DataParser")]
public class DefaultAndEnumDataParser: DataParserBase {

    
    [SerializeField] private string m_AddEnumsPath;
    [SerializeField] protected RawDataLoader m_DataLoader;
    
    public void AddEnums() {

        string path = $@"Assets\Resources\{m_NameSpace}\DataTypes\AddType.cs";
        if (string.IsNullOrEmpty(m_AddEnumsPath)) return;
        if (File.Exists(path)) {
            Debug.Log($"AddDataFile is already exist ({path})");
        }
        
        List<List<string>> data = m_DataLoader.Load(m_AddEnumsPath);

        CodeCompileUnit compileUnit = new();
        CodeNamespace codeNamespace = new(m_NameSpace);
        compileUnit.Namespaces.Add(codeNamespace);
        
        foreach (var row in data) {

            CodeTypeDeclaration newEnum = new(row[0]);
            newEnum.IsEnum = true;
            newEnum.Attributes = MemberAttributes.Public;
            newEnum.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(SerializableAttribute))
                )
            );

            int index = 0;
            foreach (var factor in row.Skip(1)) {

                newEnum.Members.Add(new CodeMemberField(typeof(int), factor)
                    { InitExpression = new CodePrimitiveExpression(index++) });

            }

            codeNamespace.Types.Add(newEnum);
        }

        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        StreamWriter wirter = new(path);
        provider.GenerateCodeFromCompileUnit(compileUnit, wirter, new CodeGeneratorOptions());
        wirter.Close();

        Debug.Log("make addData type");
    }

    public override void SetUp() {

        base.SetUp();
        AddEnums();
        AssetDatabase.Refresh();
    }
    
    public void Generate(string path) {
        //check path is correct
        if (new[] {m_NameSpace, path}.Any(string.IsNullOrEmpty)) {
            throw new NullReferenceException("NameSpace and Path must not null or empty");
        }
        
        //split data
        List<List<string>> data = m_DataLoader.Load(path);
        List<TypeAndName> header;
        (header, data) = Divide(data);
                
        //find data type
        string dataTypeName = DataTypeName(path);
        Type dataType = Type.GetType($"{m_NameSpace}.{dataTypeName}");
                
        if (dataType is null) {
            //make DataType
            GenerateDataType(dataTypeName, header);
            AssetDatabase.Refresh();
                    
        }
        else {
            Debug.Log($"already exist {dataTypeName} Type");
        }
    }
    
    public void Sync(string path) {

        //check path is correct
        if (new[] {m_NameSpace, path}.Any(string.IsNullOrEmpty)) {
            throw new NullReferenceException("NameSpace and Path must not null or empty");
        }
        
        //split data
        List<List<string>> data = m_DataLoader.Load(path);
        List<TypeAndName> header;
        (header, data) = Divide(data);
        
        //find data type
        string dataTypeName = DataTypeName(path);
        Type dataType = Type.GetType($"{m_NameSpace}.{dataTypeName}");

        if (dataType is null) {

            throw new NullReferenceException("Type isn't exist. please press Generate button");
        }
        else {
            
            SyncData(data, dataTypeName);
            Debug.Log("Sync complete");
        }
    }
}
#endif