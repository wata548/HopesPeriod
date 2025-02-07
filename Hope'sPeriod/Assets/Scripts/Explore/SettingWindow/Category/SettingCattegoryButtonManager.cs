using System;
using UnityEngine;

public class SettingCattegoryButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; }

    [SerializeField] private InventoryWindow inventory;
    public override void SelectIn(InteractButton target) {
        throw new System.NotImplementedException();
    }

    public override void SelectOut(InteractButton target) {
        throw new System.NotImplementedException();
    }
    
    private void TurnOnInventory() {
        inventory.TurnOn();
    }
    private void TurnOnSkill() {
                
        TurnOff();
    }
    private void TurnOnSave() {
               
        TurnOff();
    } 
    private void TurnOnSetting() {
               
        TurnOff();
    }
    private void TurnOnManual() {
               
        TurnOff();
    }
    private void QuitGame() {
    }

    private void TurnOff() {
        inventory.TurnOff();
    }

    public void Click(int index) {
        Action action = index switch {
        
            0 => TurnOnInventory,
            1 => TurnOnSkill,
            2 => TurnOnSave,
            3 => TurnOnSetting,
            4 => TurnOnManual,
            5 => QuitGame
        };
                
        action?.Invoke();
    }
}