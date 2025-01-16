using System;
using SpreadInfo;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum CodeType {
    Monster = 1,
    Item,
    Document,
    Key,
    SKill = 9
};

public static class ItemInfo {

    public const int CodeDigit = 1000;
    private static ItemDBDataTable table = null;

    public static bool CheckTable() {

        if (table is null) return false;
        return true;
    } 
    
    public static bool SetTable() {

        if (table is not null) return true;
        string path = "ItemTable";

        Addressables.LoadAssetAsync<ItemDBDataTable>(path).Completed += handle => 
        {
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                Debug.Log($"{path} Load succeed");
                table = handle.Result;
            }
            else {
                Debug.Log($"{path} Fail to load");
            }
        };
        
        if (table is not null) return true;
        return false;
    }

    private static ItemDBData GetData(int code) {

        if (!SetTable())
            return null;
        
        if (table.DataTable.TryGetValue(code, out ItemDBData data)) {
            return data;
        }

        throw new NullReferenceException($"This code({code}) isn't exist");
    }

    #region Property
    public static string Name(int code) {
        return GetData(code)?.Name ?? "wait"; 
    }
    public static string Description(int code) {
        return GetData(code)?.Description ?? "wait";
    }
    public static float HealHp(int code) {
        return GetData(code)?.HealHP ?? -1;
    }
    public static float HealMp(int code) {
        return GetData(code)?.HealMP ?? -1;
    }
    public static float HealsHp(int code) {
        return GetData(code)?.HealsHP ?? -1;
    }
    public static float HealsMp(int code) {
        return GetData(code)?.HealsMP ?? -1;
    }
    public static bool Revive(int code) {
        return GetData(code)?.Revive ?? false;
    }

    public static bool ReviveAll(int code) {
        return GetData(code)?.ReviveAll ?? false;
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