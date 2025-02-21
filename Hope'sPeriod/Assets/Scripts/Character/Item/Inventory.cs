using System;
using System.Collections.Generic;
using System.Linq;

public static class Inventory{
     
    public static SortedDictionary<int, int> Items{ get; private set; } = new();
    public static Dictionary<int, int> Use { get; private set; } = new();
    
    public static bool IsHave(int code, int count) {
        if (!Items.ContainsKey(code))
            return false;

        return Items[code] >= count;
    }
    
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
            .Where(value => value.Key.ToCodeType() == mask)
            .Count(value => value.Value > 0);
    }

    public static List<int> Category(CodeType mask) {
        return Items
            .Where(value => value.Key.ToCodeType() == mask && value.Value > 0)
            .Select(value => value.Key)
            .ToList();
    }
    
    public static bool UseItemBattle(int code, EachCharacterInfoBattle target = null) {
        
        if (!Items.ContainsKey(code))
            throw new NullReferenceException($"This item is yet added, try add \"{code}\"Item and retry");

        if (Items[code] <= 0) {
            return false;
        }

        if (!Use.TryAdd(code, 1)) {
            Use[code]++;
        }
        
        Items[code]--;
        ItemInfo.UseItemBattle(code, target);
        
        CallBackMainFsm();
        
        return true;
    }
    
    public static bool UseItem(int code, EachCharacterInfo target = null) {
            
            if (!Items.ContainsKey(code))
                throw new NullReferenceException($"This item is yet added, try add \"{code}\"Item and retry");
    
            if (Items[code] <= 0) {
                return false;
            }

            if (!Use.TryAdd(code, 1)) {
                Use[code]++;
            }
            
            Items[code]--;
            ItemInfo.UseItem(code, target);
            
            return true;
        }

    private static void CallBackMainFsm() {

        foreach (var character in CharactersInfoBattle.Instance.CharacterInfos) {
            character.SetEffectImage();
        }
        GameFSM.Instance.SkipState();
    }

    private static void Clear() {
        Use.Clear();
        Items.Clear();
    }

    public static void Load(SaveItem[] items,SaveUseItem[] useItems) {
        Clear();
        foreach (var item in items) {
            AddItem(item.Code, item.Amount);
        }
        foreach (var useItem in useItems) {
            Use.Add(useItem.Code, useItem.Count);
        }
    }  
}