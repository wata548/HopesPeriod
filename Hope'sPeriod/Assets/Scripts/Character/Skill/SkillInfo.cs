using SpreadInfo;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public static class SkillInfo {
        
    public const int CodeMask = 1000;
    
    private static SkillDBDataTable table = null;
    
    public static bool CheckTable() {
    
        if (table is null) return false;
        return true;
    } 
        
    public static bool SetTable() {
    
        if (table is not null) return true;
        string path = "SkillTable";
    
        Addressables.LoadAssetAsync<SkillDBDataTable>(path).Completed += handle => 
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
        
    private static SkillDBData GetData(int code) {
        
        if (!SetTable())
            return null;
                
        if (table.DataTable.TryGetValue(code, out SkillDBData data)) {
            return data;
        }
        
        throw new NullReferenceException($"This code({code}) isn't exist");
    }
        
    public static bool NeedSelect(int code) {
        
        if (!SetTable()) {
            throw new Exception("Yet load table");
            return false;
        }

        bool need = false;
        if (SkillItem(code) == true) need |= ItemInfo.NeedSelect(ToSkillItem(code));
        return false;
    }

    public static int               ToSkillItem(int code)       => code - 7000;
    public static string            Name(int code)              => GetData(code)?.Name          ?? "wait"; 
    public static string            Description(int code)       => GetData(code)?.Description   ?? "wait";
    public static int               UseHp(int code)             => GetData(code)?.UseHP         ?? -1;
    public static int               UseMp(int code)             => GetData(code)?.UseMP         ?? -1;
    public static int               Attack(int code)            => GetData(code)?.ATK           ?? -1;
    public static int               AttackDuration(int code)    => GetData(code)?.ATKCon        ?? -1;
    
    public static bool              SkillItem(int code)         => GetData(code)?.SkillItem     ?? false;
    public static int               SkillItemDuration(int code) => GetData(code)?.SkillItemCon  ?? -1;
    #endregion
}