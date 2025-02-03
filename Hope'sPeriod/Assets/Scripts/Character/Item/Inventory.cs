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

    public static List<int> Category(CodeType mask) {
        return Items
            .Where(value => (CodeType)(value.Key / ItemInfo.CodeMask) == mask && value.Value > 0)
            .Select(value => value.Key)
            .ToList();
    }
    
    public static bool UseItem(int code, EachCharacterInfoBattle target = null) {
        
        if (!Items.ContainsKey(code))
            throw new NullReferenceException($"This item is yet added, try add \"{code}\"Item and retry");

        if (Items[code] <= 0) {
            return false;
        }

        Items[code]--;
        ItemInfo.UseItem(code, target);
        
        CallBackMainFsm();
        
        return true;
    }

    private static void CallBackMainFsm() {

        GameFSM.Instance.SkipState();
    }

   
}