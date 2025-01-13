using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Inventory{
     
     public static SortedDictionary<ItemFormat, int> Items{ get; private set; } = new();

     public static void AddItem(ItemFormat item, int count) {

          if (!Items.TryAdd(item, count))
               Items[item] += count;
     }

     public static void UseItem(ItemFormat item) {

          if (!Items.ContainsKey(item))
               throw new NullReferenceException($"This item is yet added, try add \"{item.Code}\"Item and retry");

          if (Items[item] > 0) {
               Items[item]--;
          }
     }
}