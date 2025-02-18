using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "MapEventInfo")]
public class MapEventInfo : ScriptableObject {

    [SerializeField] private SerializableDictionary<int, LayerEventInfo> mapInfo;

    public string Name(int code) {
        return mapInfo[ConnectMapInfo.ToLayer(code)]
            ?.RoomInfo[code]
            ?.Name 
            ?? throw new Exception($"check about code {code}");
    }
    
    public GameObject Prefab(int code) {
        return mapInfo[ConnectMapInfo.ToLayer(code)]
            ?.RoomInfo[code]
            ?.MapPrefab
            ?? throw new Exception($"check about code {code}");
    }
    
    public bool ConnectInfo(int code, Vector2Int pos, out ConnectMapInfo info, out GameObject prefab) {

        info = null;
        prefab = null;
        
        bool result = mapInfo[ConnectMapInfo.ToLayer(code)]
                          ?.RoomInfo[code]
                          ?.MoveEventList(pos, out info) 
                      ?? throw new Exception("check about code");

        if (result) {

            prefab = mapInfo[ConnectMapInfo.ToLayer(info.ConnectMapCode)]
                ?.RoomInfo[info.ConnectMapCode]
                ?.MapPrefab;
        }
        
        return result;
    }
    public bool PassiveInfo(int code, Vector2Int pos, out EventInfo resultCode) {
        resultCode = new();
        
        bool result = mapInfo[ConnectMapInfo.ToLayer(code)]
                          ?.RoomInfo[code]
                          ?.PassiveEventList(pos, out resultCode) 
                      ?? throw new Exception("check about code");
        
        return result;
    }
    public bool AutoInfo(int code, Vector2Int pos, out EventInfo resultCode) {
        resultCode = new();
            
        bool result = mapInfo[ConnectMapInfo.ToLayer(code)]
                          ?.RoomInfo[code]
                          ?.AutoEventList(pos, out resultCode) 
                      ?? throw new Exception("check about code");
            
        return result;
    }

    public bool Item(int code, Vector2Int pos, out GetItemInfo item) {
   
        item = new();
        bool result = mapInfo[ConnectMapInfo.ToLayer(code)]
                          ?.RoomInfo[code]
                          ?.Item(pos, out item) 
                      ?? throw new Exception("check about code");
                       
        return result;
    }

    public int MeetMonster(int code) {
        return mapInfo[ConnectMapInfo.ToLayer(code)]
                   ?.RoomInfo[code]
                   ?.MeetMonster() 
               ?? throw new Exception("check about code");
    }

}

[Serializable]
public class LayerEventInfo{

    [SerializeField] private SerializableDictionary<int, RoomEventInfo> roomInfo;
    public SerializableDictionary<int, RoomEventInfo> RoomInfo => roomInfo;
}
    
[Serializable]
public class RoomEventInfo {
    [SerializeField] private string roomName;
    [SerializeField] private SerializableDictionary<Vector3Int, EventInfo> autoEventList;
    [SerializeField] private List<int> monsterList;

    [SerializeField] private float genProbability;
    [SerializeField] private SerializableDictionary<Vector3Int, GetItemInfo> itemList; 
    [SerializeField] private SerializableDictionary<Vector3Int, EventInfo> passiveEventList;
    [SerializeField] private SerializableDictionary<Vector2Int, ConnectMapInfo> moveEventList;
    [SerializeField] private GameObject mapPrefab;

    public GameObject MapPrefab => mapPrefab;
    public string Name => roomName;

    private bool IsTrueCondition(string condition) {
        string vector = @"(Item|Event|Monster|Have)\((.*)\)";
        string realNumber = @"(-?\d*)";
        bool badCondition = condition.Contains('!');
        Match match = Regex.Match(condition, vector); 
        var values= match
            .Groups[2].Value
            .Split(',');
        var type = match
            .Groups[1].Value;
        
        bool temp = false;
        if (match.Groups[1].Value == "Item") {
                            
            int mapCode = Convert.ToInt32(Regex.Match(values[0], realNumber).Groups[1].Value);
            int x = Convert.ToInt32(Regex.Match(values[1], realNumber).Groups[1].Value);
            int y = Convert.ToInt32(Regex.Match(values[2], realNumber).Groups[1].Value);
            int z = Convert.ToInt32(Regex.Match(values[3], realNumber).Groups[1].Value);
            temp = FindEventInfo.AlreadyItem(mapCode, new(x,y,z));
        }
        if (match.Groups[1].Value == "Event") {
                            
            int mapCode = Convert.ToInt32(Regex.Match(values[0], realNumber).Groups[1].Value);
            int x = Convert.ToInt32(Regex.Match(values[1], realNumber).Groups[1].Value);
            int y = Convert.ToInt32(Regex.Match(values[2], realNumber).Groups[1].Value);
            int z = Convert.ToInt32(Regex.Match(values[3], realNumber).Groups[1].Value);
            temp = FindEventInfo.AlreadyEvent(mapCode, new(x,y,z));
        }
        if (match.Groups[1].Value == "Monster") {
            int target = Convert.ToInt32(Regex.Match(values[0], realNumber).Groups[1].Value);
            int count = Convert.ToInt32(Regex.Match(values[0], realNumber).Groups[1].Value);
        
            temp = MonsterInfo.IsKill(target, count);
        }
        if (match.Groups[1].Value == "Have") {
            int target = Convert.ToInt32(Regex.Match(values[0], realNumber).Groups[1].Value);
            int count = Convert.ToInt32(Regex.Match(values[0], realNumber).Groups[1].Value);
        
            temp = Inventory.IsHave(target, count);
        }

        return !temp == badCondition;
    }
    
    public bool Item(Vector2Int v, out GetItemInfo code) {

        var availablePersibilities = itemList
            .Where(pos => pos.Key.x == v.x && pos.Key.y == v.y)
            .Select(pos => pos.Value)
            .ToList();

        foreach (var availablePersibility in availablePersibilities) {
            
            var conditions = availablePersibility.Condition.Split('|');
            bool result = true;
            foreach (var condition in conditions) {
                
                //when didn't match condition
                if (!IsTrueCondition(condition)) {
                    result = false;
                    break;
                }
            }

            if (result) {
                code = availablePersibility;
                return true;
            }
        }

        code = null;
        return false;
    }
    
    public bool MoveEventList(Vector2Int v, out ConnectMapInfo mapInfo) {
        
        mapInfo = null;
        
        bool result = moveEventList?.ContainsKey(v) ?? false;
        if (result) {
            mapInfo = moveEventList[v];
        }

        return result;
    }

    public bool AutoEventList(Vector2Int v, out EventInfo code) {
        var availablePersibilities = autoEventList
            .Where(pos => pos.Key.x == v.x && pos.Key.y == v.y)
            .Select(pos => pos.Value)
            .ToList();

        foreach (var availablePersibility in availablePersibilities) {
            
            var conditions = availablePersibility.Condition.Split('|');
            bool result = true;
            foreach (var condition in conditions) {
                
                //when didn't match condition
                if (!IsTrueCondition(condition)) {
                    result = false;
                    break;
                }
            }

            if (result) {
                code = availablePersibility;
                return true;
            }
        }

        code = null;
        return false;
    }
    public bool PassiveEventList(Vector2Int v, out EventInfo code) {
        var availablePersibilities = passiveEventList
            .Where(pos => pos.Key.x == v.x && pos.Key.y == v.y)
            .Select(pos => pos.Value)
            .ToList();

        foreach (var availablePersibility in availablePersibilities) {
            
            var conditions = availablePersibility.Condition.Split('|');
            bool result = true;
            foreach (var condition in conditions) {
                
                //when didn't match condition
                if (!IsTrueCondition(condition)) {
                    result = false;
                    break;
                }
            }

            if (result) {
                code = availablePersibility;
                return true;
            }
        }

        code = null;
        return false;
    }

    public int MeetMonster() {
        int code = 0;
        if (1 - Random.Range(0, 1f) <= genProbability)
            code = monsterList[Random.Range(0, monsterList.Count)];

        return code;
    }
}

[Serializable]
public class GetItemInfo {

    [SerializeField] private int code;
    [SerializeField] private int count;
    [SerializeField] private string condition;
    
    public int Code => code;
    public int Count => count;
    public string Condition => condition;

    public GetItemInfo() {
        code = 0;
        count = 0;
        condition = "";
    }
}

[Serializable]
public class EventInfo {

    [SerializeField] private int code;
    [SerializeField] private string condition;
    
    public int Code => code;
    public string Condition => condition;

    public EventInfo() {
        code = 0;
        condition = "";
    }
}
    
[Serializable]
public class ConnectMapInfo {

    [SerializeField] private int connectMapCode;
    [SerializeField] private Vector2Int connectPos;

    public int ConnectMapCode => connectMapCode;
    public Vector2Int ConnectPos => connectPos;

    //8401 -> 84 -> 4
    public static int ToLayer(int code) => (code / 100) % 10; 
    
    public ConnectMapInfo(int mapCode, Vector2Int pos) {
        connectMapCode = mapCode;
        connectPos = pos;
    }
    public ConnectMapInfo() {}
}