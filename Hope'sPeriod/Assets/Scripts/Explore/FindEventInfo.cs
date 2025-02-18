using System.Collections.Generic;
using UnityEngine;

public static class FindEventInfo {

    public static Dictionary<int, HashSet<Vector3Int>> FindItemInfos { get; private set; } = new();
    public static Dictionary<int, HashSet<Vector3Int>> FindEventInfos { get; private set; } = new();

    public static void FindItem(int mapCode, Vector3Int pos) {
        FindItemInfos.TryAdd(mapCode, new());

        FindItemInfos[mapCode].Add(pos);
    }
    
    public static void FindEvent(int mapCode, Vector3Int pos) {
        FindEventInfos.TryAdd(mapCode, new());
    
        FindEventInfos[mapCode].Add(pos);
    }

    public static bool AlreadyItem(int mapCode, Vector3Int pos) => FindItemInfos?[mapCode]?.Contains(pos) ?? false;
    public static bool AlreadyEvent(int mapCode, Vector3Int pos) => FindEventInfos?[mapCode]?.Contains(pos) ?? false;
    
}