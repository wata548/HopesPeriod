using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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
                          ?.PassiveEventList(pos, out resultCode, out var resultPos) 
                      ?? throw new Exception("check about code");

        if (result)
            AlreadyFindEventInfo.FindEvent(code, resultPos);
        
        return result;
    }
    public bool AutoInfo(int code, Vector2Int pos, out EventInfo resultCode) {
        resultCode = new();
            
        bool result = mapInfo[ConnectMapInfo.ToLayer(code)]
                          ?.RoomInfo[code]
                          ?.AutoEventList(pos, out resultCode, out var resultPos) 
                      ?? throw new Exception("check about code");
            
        if(result)
            AlreadyFindEventInfo.FindEvent(code, resultPos);
        
        return result;
    }

    public bool Item(int code, Vector2Int pos, out GetItemInfo item) {

        item = new();
        bool result = mapInfo[ConnectMapInfo.ToLayer(code)]
                          ?.RoomInfo[code]
                          ?.Item(pos, out item, out var itemPos) 
                      ?? throw new Exception("check about code");

        if (result)
            AlreadyFindEventInfo.FindItem(code, itemPos);
        
        return result;
    }

    public int MeetMonster(int code) {
        return mapInfo[ConnectMapInfo.ToLayer(code)]
                   ?.RoomInfo[code]
                   ?.MeetMonster() 
               ?? throw new Exception("check about code");
    }

    public List<(Vector3Int, GetItemInfo)> Items(int code) {
        return mapInfo[ConnectMapInfo.ToLayer(code)]
                   ?.RoomInfo[code].Items
               ?? throw new Exception($"check about code {code}");
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


    public List<(Vector3Int, GetItemInfo)> Items => itemList.Select(fac => (fac.Key, fac.Value)).ToList();
    public bool Item(Vector2Int v, out GetItemInfo code, out Vector3Int pos) {

        var availablePersibilities = itemList
            .Where(pos => pos.Key.x == v.x && pos.Key.y == v.y)
            .ToList();

        foreach (var availablePersibility in availablePersibilities) {
            
            bool result = ScriptCodeInterpreter
                .Interpret(availablePersibility.Value.Condition)
                .ToCondition();

            if (result) {
                code = availablePersibility.Value;
                pos = availablePersibility.Key;
                return true;
            }
        }

        pos = Vector3Int.zero;
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

    public bool AutoEventList(Vector2Int v, out EventInfo code, out Vector3Int pos ) {
        var availablePersibilities = autoEventList
            .Where(pos => pos.Key.x == v.x && pos.Key.y == v.y)
            .ToList();

        foreach (var availablePersibility in availablePersibilities) {
            bool result = ScriptCodeInterpreter
                .Interpret(availablePersibility.Value.Condition)
                .ToCondition();
         
            if (result) {
                code = availablePersibility.Value;
                pos = availablePersibility.Key;
                return true;
            }   
        }

        pos = Vector3Int.zero;
        code = null;
        return false;
    }
    public bool PassiveEventList(Vector2Int v, out EventInfo code, out Vector3Int pos) {
        var availablePersibilities = passiveEventList
            .Where(pos => pos.Key.x == v.x && pos.Key.y == v.y)
            .ToList();

        foreach (var availablePersibility in availablePersibilities) {
            
            bool result = ScriptCodeInterpreter
                .Interpret(availablePersibility.Value.Condition)
                .ToCondition();
            
            if (result) {
                pos = availablePersibility.Key;
                code = availablePersibility.Value;
                return true;
            }
        }

        pos = Vector3Int.zero;
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

    public GetItemInfo(int code, int count = 1) {
        this.code = code;
        this.count = count;
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