using System;
using UnityEngine;
using System.Collections.Generic;
using SpreadInfo;

public class InventoryTest: MonoBehaviour {

    private void Start() {

        Inventory.AddItem(2005, 4);
        Inventory.AddItem(2001, 1);
        Inventory.AddItem(2002, 2);
        Inventory.AddItem(2003, 3);
        Inventory.AddItem(2004, 2);
        Inventory.AddItem(2006, 2);
        
    }
}