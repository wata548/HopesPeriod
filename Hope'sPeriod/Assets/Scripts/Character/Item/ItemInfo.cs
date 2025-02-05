using System;
using SpreadInfo;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum CodeType {
    Monster = 1,
    Item = 2,
    Document = 3,
    Key = 4,
    Actor = 5,
    Script = 6,
    SimpleInteract = 7,
    Map = 8,
    SKill = 9
};

public static class ItemInfo {

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

    #region Property
    
    private static ItemDBData GetData(int code) {
    
        if (!SetTable())
            return null;
            
        if (table.DataTable.TryGetValue(code, out ItemDBData data)) {
            return data;
        }
    
        throw new NullReferenceException($"This code({code}) isn't exist");
    }
    
    public static bool NeedSelect(int code) {
    
        if (!SetTable()) {
            throw new Exception("Yet load table");
            return false;
        }
        bool hp = HealHp(code) != 0;
        bool mp = HealMp(code) != 0;
        bool effectTarget = EffectTarget(code) == EffectTargetType.Select;
        
        return hp || mp || effectTarget;
    }
        
    public static string            Name(int code)              => GetData(code)?.Name          ?? "wait"; 
    public static string            Description(int code)       => GetData(code)?.Description   ?? "wait";
    public static float             HealHp(int code)            => GetData(code)?.HealHP        ?? -1;
    public static float             HealMp(int code)            => GetData(code)?.HealMP        ?? -1;
    public static float             HealsHp(int code)           => GetData(code)?.HealsHP       ?? -1;
    public static float             HealsMp(int code)           => GetData(code)?.HealsMP       ?? -1;
    public static bool              Revive(int code)            => GetData(code)?.Revive        ?? false;
    public static bool              ReviveAll(int code)         => GetData(code)?.ReviveAll     ?? false;
    public static EffectType        Effect(int code)            => GetData(code)?.Effect        ?? EffectType.None;
    public static float             EffectPower(int code)       => GetData(code)?.EffectPower   ?? -1;
    public static int               EffectDuration(int code)    => GetData(code)?.EffectCon     ?? -1;
    public static EffectTargetType  EffectTarget(int code)      => GetData(code)?.EffectTarget  ?? EffectTargetType.None;
    public static float             DefencePower(int code)      => GetData(code)?.DEFPower      ?? -1f;
    public static DefenceType       DefenceType(int code)       => GetData(code)?.DEFType       ?? SpreadInfo.DefenceType.None;
    public static bool              DefenceReflect(int code)    => GetData(code)?.DEFReflect    ?? false;
    public static float             Attract(int code)           => GetData(code)?.ATR           ?? -1;
    public static int               AttractDuration(int code)   => GetData(code)?.ATRCon        ?? -1;

    public static string SimpleTag(int code) {

        string result = "";
        //It find detail type name 
        //if (EffectInfo.MatchKorean.TryGetValue(ItemInfo.Effect(itemCode), out string korean)) {
        if (Effect(code) != EffectType.None) result += "효과 ";
        
        bool healHp = HealHp(code) != 0 || HealsHp(code) != 0;
        bool healMp = HealMp(code) != 0 || HealsMp(code) != 0; 
        if ( healHp || healMp) result += "회복";
        if (Attract(code) > 0 && !ItemInfo.DefenceReflect(code)) result += "도발 ";
        
        if (DefenceType(code) != SpreadInfo.DefenceType.None) {
            if (DefenceReflect(code)) result += "반사";
            else result += "방어 ";
        }

        return result;
    }
    
    #endregion

    public static bool UseAble(int code, EachCharacterInfo user = null) {
        if (!SetTable()) {
            throw new Exception("Yet load table");
            return false;
        }
           
        ItemDBData item = GetData(code);
        if (user is null) return true;
        if (!user.Dead) return true; 
        if (item.Revive && item.HealHP > 0) return true;
        if (item.ReviveAll && item.HealsHP > 0) return true;
        return false;
    }

    public static bool HealItemNeedSelect(int code) {
        if (!SetTable()) {
            throw new Exception("Yet load table");
            return false;
        }
        bool hp = HealHp(code) != 0;
        bool mp = HealMp(code) != 0;
                
        return hp || mp;
    }
    
    public static void UseItemBattle(int code, EachCharacterInfoBattle user = null) {
        if (!SetTable()) {
            throw new Exception("Yet load table");
            return;
        }
   
        ItemDBData item = GetData(code);
           
        foreach (var character in CharactersInfoBattle.Instance.CharacterInfos) {
            if(item.HealsHP != 0) character.HealHp(item.HealsHP, item.ReviveAll);
            if(item.HealsMP != 0 && !character.Dead) character.HealMp(item.HealsMP);
            
            if (character.Dead || item.EffectTarget != EffectTargetType.AllCharacter)
                continue;
            
            if(item.Effect != EffectType.None) character.SetEffect(code);
            if (item.DEFPower != 0) character.SetShield(code);
        }
           
        if(user is not null) {
            if(item.HealHP != 0) user.HealHp(item.HealHP, item.Revive);
            if(item.HealMP != 0) user.HealMp(item.HealMP);
            if(item.ATR != 0) user.SetAttract(code);

            if (item.EffectTarget == EffectTargetType.AllCharacter) return;

            if (item.Effect != EffectType.None) user.SetEffect(code);
            if (item.DEFPower != 0) user.SetShield(code);

        }
    }
    
    public static void UseItem(int code, EachCharacterInfo user = null) {
        if (!SetTable()) {
            throw new Exception("Yet load table");
            return;
        }
       
        ItemDBData item = GetData(code);
               
        foreach (var character in CharacterInfoInventory.Instance.Characterinfos) {
            if(item.HealsHP != 0) character.Info.HealHp(item.HealsHP, item.ReviveAll);
            if(item.HealsMP != 0 && !character.Info.Dead) character.Info.HealMp(item.HealsMP);
        }
               
        if(user is not null) {
            if(item.HealHP != 0) user.HealHp(item.HealHP, item.Revive);
            if(item.HealMP != 0) user.HealMp(item.HealMP);
        }
    }
}