using System;
using SpreadInfo;
using UnityEditor;
using UnityEngine;

public static class ItemInfo {

    private static ItemDBDataTable table = null;

    private static void SetTable() {

        if (table is not null) return;

        string path = "Assets/SpreadInfo/Generated/ItemDBDataTable.asset";
        table = AssetDatabase.LoadAssetAtPath<ItemDBDataTable>(path);
    }

    private static ItemDBData GetData(int code) {

        SetTable();
        if (table.DataTable.TryGetValue(code, out ItemDBData data)) {
            return data;
        }

        throw new NullReferenceException($"This code({code}) isn't exist");
    }

    #region Property
    public static string Name(int code) {
        return GetData(code).Name;
    }
    public static string Description(int code) {
        return GetData(code).Description;
    }
    public static float HealHp(int code) {
        return GetData(code).HealHP;
    }
    public static float HealMp(int code) {
        return GetData(code).HealMP;
    }
    public static float HealsHp(int code) {
        return GetData(code).HealsHP;
    }
    public static float HealsMp(int code) {
        return GetData(code).HealsMP;
    }
    public static bool Revive(int code) {
        return GetData(code).Revive;
    }

    public static bool ReviveAll(int code) {
        return GetData(code).ReviveAll;
    }
    #endregion

    public static bool NeedSelect(int code) {

        SetTable();
        bool hp = HealHp(code) != 0;
        bool mp = HealMp(code) != 0;
        return hp || mp;
    }
    
    public static void UseItem(int code, ControleCharacterInfo characters, ControleEachCharacterInfo user = null) {
        SetTable();

        ItemDBData item = GetData(code);
        
        foreach (var characterInfo in characters.CharacterInfos) {
            characterInfo.HealHp(item.HealsHP, item.ReviveAll);
            characterInfo.HealMp(item.HealMP);
        }
        
        if(user is not null) {
            user.HealHp(item.HealHP, item.Revive);
            user.HealMp(item.HealMP);
        }
    }
}