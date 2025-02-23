using System.Collections.Generic;
using System;
using UnityEngine;

public static class MonsterInfo {

    public static Dictionary<int, int> Monsters { get; private set; } = new();
    private static int code = 0;
    private static GameObject monster;
    public static bool IsBoss { get; private set; } = false;

    public static bool IsFirstBattle => Monsters.Count <= 0;
    private static void Clear() {
        Monsters.Clear();
    }
    private static void KillMonster(int code) {
        if (!Monsters.TryAdd(code, 1)) {
            Monsters[code]++;
        }
    }
    public static void Load(SaveMonster[] datas) {
        Clear();
        
        foreach (var data in datas) {
            Monsters.Add(data.Code, data.KillCount);
        }
    }
    public static bool IsKill(int code, int count) {
        if (!Monsters.ContainsKey(code))
            return false;

        return Monsters[code] >= count;
    }
    public static void SetMonster(int code) {
            
        MonsterInfo.code = code;
        monster = Resources.Load<GameObject>($"Monster/M{code}/M{code}");
            
        if (monster is null)
            throw new Exception($"This monster({code}) isn't exist");
    
        IsBoss = monster?.GetComponent<Monster>().IsBoss ?? false;
        if (IsBoss) {
            var playerInfo = CharacterInfoInventory.Instance.RawCharacterInfos.ToArray();
            var pos = TilePlayerPhysics.Instance.Object.transform.localPosition;
            SaveData.Save(-1, playerInfo, ChapterInfo.Chapter, CheckEvent.MapCode, pos);
        }
    
    }
    public static void Kill() {
        GameObject.Destroy(Monster.Instance.gameObject);
        KillMonster(code);
        code = 0;
    }
    public static void MakeMonster() {
        GameObject.Instantiate(monster);
    }
}