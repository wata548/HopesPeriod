using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Inventory{
     
     public static SortedDictionary<int, int> Items{ get; private set; } = new();

     public static void AddItem(int item, int count) {

          if (!Items.TryAdd(item, count))
               Items[item] += count;
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