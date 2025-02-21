using System;
using UnityEngine;

public static class CurrentMapInfo {
    private static int code = 8401;
    private static Vector2 pos = Vector2Int.zero;

    public static void SetData(int code, Vector2Int pos) {
        CurrentMapInfo.code = code;
        CurrentMapInfo.pos = pos;
    }

    public static void SetData(int code, Vector2 pos) {
        CurrentMapInfo.code = code;
        CurrentMapInfo.pos = pos;
    }

    public static (int code, Vector2 pos) Load() {
        return (code, pos);
    }
}

public static class CurrentMonsterInfo {

    private static int code = 0;
    private static GameObject monster;
    public static bool IsBoss { get; private set; } = false;
    
    public static void SetMonster(int code) {
        
        CurrentMonsterInfo.code = code;
        monster = Resources.Load<GameObject>($"Monster/M{code}/M{code}");
        
        if (monster is null)
            throw new Exception($"This monster({code}) isn't exist");

        IsBoss = monster?.GetComponent<Monster>().IsBoss ?? false;
        if (IsBoss) {
            var playerInfo = CharacterInfoInventory.Instance.RawCharacterInfos.ToArray();
            var pos = TilePlayerPhysics.Instance.Object.transform.localPosition;
            SaveData.Save(-1, playerInfo, ChapterInfo.Chapter, CheckEvent.MapCode, pos);
        }

        MakeMonster();
    }

    public static void MakeMonster() {
        GameObject.Instantiate(monster);
    }
}