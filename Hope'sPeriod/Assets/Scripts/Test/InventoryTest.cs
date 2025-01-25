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
        Inventory.AddItem(2099, 2);
        ControleCharacterInfo.Instance.CharacterInfo(0).SetSkill(0, 9101);
        ControleCharacterInfo.Instance.CharacterInfo(0).SetSkill(1, 9102);
        ControleCharacterInfo.Instance.CharacterInfo(0).SetSkill(2, 9103);
    }
}