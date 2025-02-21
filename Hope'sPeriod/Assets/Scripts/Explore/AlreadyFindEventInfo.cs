using System.Collections.Generic;
using UnityEngine;

public static class AlreadyFindEventInfo {

    public static Dictionary<int, HashSet<Vector3Int>> FindItemInfos { get; private set; } = new();
    public static Dictionary<int, HashSet<Vector3Int>> FindEventInfos { get; private set; } = new();
    public static HashSet<int> Events = new();
    
    private static void Clear() {
        FindItemInfos.Clear();
        FindEventInfos.Clear();
        Events.Clear();
    }

    public static void Load(SaveFindItem[] findItems, SaveFindEvent[] findEvents, List<int> events) {
        Clear();
        
        foreach (var findItem in findItems) {
            AlreadyFindEventInfo.FindItem(findItem.Code, new(findItem.X, findItem.Y, findItem.Z));
        }
        foreach (var findEvent in findEvents) {
            AlreadyFindEventInfo.FindEvent(findEvent.Code, new(findEvent.X, findEvent.Y, findEvent.Z));
        }

        foreach (var @event in events) {
            Events.Add(@event);
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
    public static void FindEvent(int code) => Events.Add(code);

    public static bool AlreadyItem(int mapCode, Vector3Int pos) => FindItemInfos?[mapCode]?.Contains(pos) ?? false;
    public static bool AlreadyEvent(int mapCode, Vector3Int pos) => FindEventInfos?[mapCode]?.Contains(pos) ?? false;
    
}