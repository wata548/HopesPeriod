using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using SpreadInfo;
using UnityEngine;

public static class Inventory{
     
    public static SortedDictionary<int, int> Items{ get; private set; } = new();

    public static void AddItem(int code, int count) {

        if (!Items.TryAdd(code, count))
            Items[code] += count;
    }

    public static int Count(int code) {

        if (!Items.TryGetValue(code, out int value))
            throw new NullReferenceException($"This Item is not exist {code}");

        return value;
    }

    public static int Kind() {

        return Items.Count(value => value.Value > 0);
    }

    public static int Kind(CodeType mask) {
        return Items
            .Where(value => (CodeType)(value.Key / ItemInfo.CodeMask) == mask)
            .Count(value => value.Value > 0);
    }
     
    public static bool UseNonEffectItem(int code, ControleEachCharacterInfo target = null) {

        if (!Items.ContainsKey(code))
            throw new NullReferenceException($"This item is yet added, try add \"{code}\"Item and retry");

        if (Items[code] <= 0) {
            return false;
        }

          
        Items[code]--;
        ItemInfo.UseItem(code, target);
        return true;
    }

    public static bool UseItem(int code, ControleEachCharacterInfo target = null) {
        
        bool result = UseNonEffectItem(code, target);
        ApplyEffect(code, target);

        CallBackMainFsm();
        return result;
    }

    private static void ApplyEffect(int code, ControleEachCharacterInfo target = null) {

        Debug.Log(target);
        
        var effect = ItemInfo.Effect(code);
        var effectTarget = ItemInfo.EffectTarget(code);
        if (effectTarget == EffectTargetType.None) return;
          
        if (effectTarget >= EffectTargetType.Heail) {
            int index = effectTarget - EffectTargetType.Heail;
            target = ControleCharacterInfo.Instance.CharacterInfo(index);
        }


        if (effectTarget == EffectTargetType.AllCharacter) {

            foreach (var character in ControleCharacterInfo.Instance.CharacterInfos().Where(character => !character.Dead)) {
                
                character.SetEffect(code);
                character.SetShield(code);
                character.SetAttract(code);
            }
        } 
        
        target?.SetEffect(code);
        target?.SetShield(code);
        target?.SetAttract(code);
    }
     

    private static void CallBackMainFsm() {

        GameFSM.Instance.SkipState();
    }

   
}