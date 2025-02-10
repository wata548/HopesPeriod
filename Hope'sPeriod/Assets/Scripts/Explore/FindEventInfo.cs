using System.Collections.Generic;
using UnityEngine;

public static class FindEventInfo {

    public static Dictionary<int, HashSet<Vector2Int>> FindItemInfos { get; private set; } = new();
    public static Dictionary<int, HashSet<Vector2Int>> FindEventInfos { get; private set; } = new();

    public static void FindItem(int mapCode, Vector2Int pos) {
        FindItemInfos.TryAdd(mapCode, new());

        FindItemInfos[mapCode].Add(pos);
    }
    
    public static void FindEvent(int mapCode, Vector2Int pos) {
        FindEventInfos.TryAdd(mapCode, new());
    
        FindEventInfos[mapCode].Add(pos);
    }
    
}