using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(menuName = "LoadData/Loader/SpreadSheet")]
public class SpreadSheetLoader: RawDataLoader{

    [SerializeField] private string m_SpreadSheetID;
    [SerializeField] private string m_ApiKey;

    private class SpreadSheetType {

        public string range;
        public string majorDimension;
        public List<List<string>> values;
    }
    
    public override List<List<string>> Load(string path) {

        HttpClient client = new();
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{m_SpreadSheetID}/values/{path}?key={m_ApiKey}";
        var data = client.GetStringAsync(url).Result;
        List<List<string>> result = JsonConvert.DeserializeObject<SpreadSheetType>(data).values;

        return result;
    }
}