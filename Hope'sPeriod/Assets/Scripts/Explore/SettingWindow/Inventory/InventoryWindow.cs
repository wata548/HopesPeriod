using System;
using UnityEngine;

public class InventoryWindow: MonoBehaviour {

    [SerializeField] private CategoryButtonManager category;
    [SerializeField] private InventoryButtonManager itemButton;
    [SerializeField] private InventoryTargetButtonManager target;
    [SerializeField] private UseButtonManager itemInfo;
    [SerializeField] private GameObject inventory;
    
    public void TurnOn() {

        inventory.SetActive(true);
        foreach (var info in CharacterInfoInventory.Instance.Characterinfos) {
            info.TurnOn();
        }
        
        category.TurnOn();
        itemButton.TurnOn();
        itemInfo.TurnOn();
        
        target.TurnOff();
    }

    public void TurnOff() {
        inventory.SetActive(false);

        category.TurnOff();
        itemButton.TurnOff();
        itemInfo.TurnOff();
        target.TurnOff();
    }

}