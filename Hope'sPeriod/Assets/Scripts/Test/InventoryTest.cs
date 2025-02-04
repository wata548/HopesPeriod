using System;
using UnityEngine;
using System.Collections.Generic;
using SpreadInfo;

public class InventoryTest: MonoBehaviour {

    private void Start() {

        Inventory.AddItem(2005, 4);
        Inventory.AddItem(2103, 4);
        Inventory.AddItem(2203, 4);
        Inventory.AddItem(2001, 1);
        Inventory.AddItem(2002, 2);
        Inventory.AddItem(2003, 3);
        Inventory.AddItem(2004, 2);
        for(int i = 2006; i <= 2019; i++)
            Inventory.AddItem(i, 2);
        //TestSetSkill();
    }

    private void TestSetSkill() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 4; j++) {
                       
                CharactersInfo.Instance.CharacterInfo(i).SetSkill(j, 9000 + (i+1) * 100 + j + 1);
            }
               
        } 
    }
}