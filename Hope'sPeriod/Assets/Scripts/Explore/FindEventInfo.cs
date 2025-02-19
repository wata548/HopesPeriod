using System.Collections.Generic;
using UnityEngine;

public static class FindEventInfo {

    public static Dictionary<int, HashSet<Vector3Int>> FindItemInfos { get; private set; } = new();
    public static Dictionary<int, HashSet<Vector3Int>> FindEventInfos { get; private set; } = new();
    public static HashSet<int> Events = new();
    
    private static void Clear() {
        FindItemInfos.Clear();
        FindEventInfos.Clear();
    }

    public static void Load(SaveFindItem[] findItems, SaveFindEvent[] findEvents) {
        Clear();
        
        foreach (var findItem in findItems) {
            FindEventInfo.FindItem(findItem.Code, new(findItem.X, findItem.Y, findItem.Z));
        }
        foreach (var findEvent in findEvents) {
            FindEventInfo.FindEvent(findEvent.Code, new(findEvent.X, findEvent.Y, findEvent.Z));
        }
    }
    
    public static void FindItem(int mapCode, Vector3Int pos) {
        FindItemInfos.TryAdd(mapCode, new());

        FindItemInfos[mapCode].Add(pos);
    }
    
    public static void FindEvent(int mapCode, Vector3Int pos) {
        FindEventInfos.TryAdd(mapCode, new());
    
        FindEventInfos[mapCode].Add(pos);
    }

    public static bool AlreadyEvent(int code) => Events.Contains(code);
    public static void StartEvent(int code) => Events.Add(code);

    public static bool AlreadyItem(int mapCode, Vector3Int pos) => FindItemInfos?[mapCode]?.Contains(pos) ?? false;
    public static bool AlreadyEvent(int mapCode, Vector3Int pos) => FindEventInfos?[mapCode]?.Contains(pos) ?? false;
    
}