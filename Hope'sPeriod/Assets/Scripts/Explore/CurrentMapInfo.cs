using UnityEngine;

public static class CurrentMapInfo {
    private static int code = 8401;
    private static Vector2Int pos = Vector2Int.zero;

    public static void SetData(int code, Vector2Int pos) {
        CurrentMapInfo.code = code;
        CurrentMapInfo.pos = pos;
    }

    public static (int code, Vector2Int pos) Load() {
        return (code, pos);
    }
}