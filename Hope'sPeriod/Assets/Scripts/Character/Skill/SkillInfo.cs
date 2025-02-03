using SpreadInfo;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.TextCore.Text;

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

    public static void UseSkill(int index, int code, int targetIndex) {

        var user = ControleCharacterInfo.Instance.CharacterInfo(index);

        int useHp = UseHp(code);
        int useMp = UseMp(code);
        int damage = Attack(code);
        
        if(useHp != 0) user.GetDamage(useHp);
        if(useMp != 0) user.UseMp(useMp);

        if (damage != 0) {

            var attackUp = user.GetEffect(EffectType.AttackUp);
            
            if (attackUp != null) {

                damage = (int)Mathf.Ceil(attackUp.Power * damage);
            }
            MonsterSlider.Instance.GetDamage(damage);
        }
        
        if (SkillItem(code)) {
        
            var targetType = ItemInfo.EffectTarget(ToSkillItem(code));
            ControleEachCharacterInfo target;
        
            target = targetType switch {
        
                EffectTargetType.None => null,
                EffectTargetType.AllCharacter => null,
                EffectTargetType.Select => ControleCharacterInfo.Instance.CharacterInfo(targetIndex),
                EffectTargetType.User => ControleCharacterInfo.Instance.CharacterInfo(index),
                _ => ControleCharacterInfo.Instance.CharacterInfo(targetType - EffectTargetType.Heail)
            };
        
            ItemInfo.UseItem(ToSkillItem(code), target);
        }
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
        if (SkillItem(code)) need = ItemInfo.NeedSelect(ToSkillItem(code));
        return need;
    }

    public static bool NeedSelect(int code, out int itemCode) {
        if (!SetTable()) {
            throw new Exception("Yet load table");
            return false;
        }
        
        itemCode = 0;
        if (!SkillItem(code)) return false;
        
        itemCode = ToSkillItem(code);
        return ItemInfo.NeedSelect(itemCode);
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

    public static bool Useable(int characterIndex, int code) {
        
        var character = ControleCharacterInfo.Instance.CharacterInfo(characterIndex);

        bool useableHp = UseHp(code) <= character.CurrentHp;
        bool useableMp = UseMp(code) <= character.CurrentMp;

        return useableHp && useableMp;
    }
    
    public static string SimpleTag(int code) {
        string result = "";
        int value;
        if ((value = UseHp(code)) > 0) result += $"HP -{value} ";
        if ((value = UseMp(code)) > 0) result += $"MP -{value} ";
        if ((value = Attack(code)) > 0) result += $"ATK {value} ";
        if (SkillItem(code)) {

            int itemCode = ToSkillItem(code);
            result += ItemInfo.SimpleTag(itemCode);
        }

        return result;
    }
    #endregion
}