using UnityEngine;

public static class SaveMapInfo {
    private static int code = 8401;
    private static Vector2Int pos = Vector2Int.zero;

    public static void Save(int code, Vector2Int pos) {
        SaveMapInfo.code = code;
        SaveMapInfo.pos = pos;
    }

    public static (int code, Vector2Int pos) Load() {
        Debug.Log(code);
        return (code, pos);
    }
}