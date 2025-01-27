using System;
using UnityEngine;
using System.Collections.Generic;
using SpreadInfo;

public class InventoryTest: MonoBehaviour {

    private void Start() {

        Inventory.AddItem(2005, 4);
        Inventory.AddItem(2203, 4);
        Inventory.AddItem(2001, 1);
        Inventory.AddItem(2002, 2);
        Inventory.AddItem(2003, 3);
        Inventory.AddItem(2004, 2);
        ControleCharacterInfo.Instance.CharacterInfo(0).SetSkill(0, 9101);
        ControleCharacterInfo.Instance.CharacterInfo(0).SetSkill(1, 9102);
        ControleCharacterInfo.Instance.CharacterInfo(0).SetSkill(2, 9103);
        
        ControleCharacterInfo.Instance.CharacterInfo(1).SetSkill(0, 9201);
        ControleCharacterInfo.Instance.CharacterInfo(1).SetSkill(1, 9202);
        ControleCharacterInfo.Instance.CharacterInfo(1).SetSkill(2, 9203);
                
        ControleCharacterInfo.Instance.CharacterInfo(2).SetSkill(0, 9301);
        ControleCharacterInfo.Instance.CharacterInfo(2).SetSkill(1, 9302);
        ControleCharacterInfo.Instance.CharacterInfo(2).SetSkill(2, 9303);
    }
}