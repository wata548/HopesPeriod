using System;
using UnityEngine;
using System.Collections.Generic;
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

    public bool PassiveInfo(int code, Vector2Int pos, out int resultCode) {
        resultCode = 0;
        
        bool result = mapInfo[ConnectMapInfo.ToLayer(code)]
                          ?.RoomInfo[code]
                          ?.PassiveEventList(pos, out resultCode) 
                      ?? throw new Exception("check about code");
        
        return result;
    }
    
    public bool AutoInfo(int code, Vector2Int pos, out int resultCode) {
        resultCode = 0;
            
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
    [SerializeField] private SerializableDictionary<Vector2Int, int> autoEventList;
    [SerializeField] private List<int> monsterList;

    [SerializeField] private float genProbability;
    [SerializeField] private SerializableDictionary<Vector2Int, GetItemInfo> itemList; 
    [SerializeField] private SerializableDictionary<Vector2Int, int> passiveEventList;
    [SerializeField] private SerializableDictionary<Vector2Int, ConnectMapInfo> moveEventList;
    [SerializeField] private GameObject mapPrefab;

    public GameObject MapPrefab => mapPrefab;
    public string Name => roomName;

    public bool Item(Vector2Int v, out GetItemInfo code) {
        code = new();
        bool result = itemList?.ContainsKey(v) ?? false;
        if (result) {
            code = itemList[v];
        }
        
        return result;
    }
    
    public bool MoveEventList(Vector2Int v, out ConnectMapInfo mapInfo) {
        
        mapInfo = null;
        
        bool result = moveEventList?.ContainsKey(v) ?? false;
        if (result) {
            mapInfo = moveEventList[v];
        }

        return result;
    }

    public bool AutoEventList(Vector2Int v, out int code) {
        code = 0;
        bool result = autoEventList?.ContainsKey(v) ?? false;
        if (result) code = autoEventList[v];
        
        return result;
    }
    public bool PassiveEventList(Vector2Int v, out int code) {
        code = 0;
        bool result = passiveEventList?.ContainsKey(v) ?? false;
        if (result) code = passiveEventList[v]; 
                
        return result;
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
    
    public int Code => code;
    public int Count => count;

    public GetItemInfo() {
        code = 0;
        count = 0;
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