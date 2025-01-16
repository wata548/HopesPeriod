using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
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
               .Where(value => (CodeType)(value.Key / ItemInfo.CodeDigit) == mask)
               .Count(value => value.Value > 0);
     }
     
     public static bool UseItem(int code) {

          if (!Items.ContainsKey(code))
               throw new NullReferenceException($"This item is yet added, try add \"{code}\"Item and retry");

          if (Items[code] <= 0) {
               return false;
          }

          Items[code]--;
          //ItemInfo.UseItem(code);
          return true;
     }
}