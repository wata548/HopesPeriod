using System.Numerics;
using UnityEngine;

public class MapEvent : MonoBehaviour
{
    [SerializeField] SerializableDictionary<Vector2Int, int> autoEventList; 
    [SerializeField] SerializableDictionary<Vector2Int, int> passiveEventList;
    [SerializeField] SerializableDictionary<Vector2Int, string> moveEventList;


    public bool MoveEventList(Vector2Int v, out string mapPrefab) {
        
        mapPrefab = null;
        bool result = moveEventList?.ContainsKey(v) ?? false;
        if (result) mapPrefab = moveEventList[v];

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
}
