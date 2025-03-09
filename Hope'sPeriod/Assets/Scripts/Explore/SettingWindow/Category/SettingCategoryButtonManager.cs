using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SettingCategoryButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; }

    [SerializeField] private InventoryWindow inventory;
    [SerializeField] private CharacterSelectButtonManager skillCharacterShower;
    [SerializeField] private SkillSelectButtonManager skillListShower; 
    [SerializeField] private LoadButtonOnPlayManager save;
    [SerializeField] private TutorialButtonManager tutorial;

    public override void SetInteractable(bool interactable) {
        base.SetInteractable(interactable);
        if (interactable)
            SettingCategoryButton.SetColor(Parse(buttons[0]).Context);
    }

    public override void SelectIn(InteractButton target) { }
    public override void SelectOut(InteractButton target) { }

    private void TurnOnInventory() {
        TurnOff();
        inventory.TurnOn();
    }
    private void TurnOnSkill() {
                
        TurnOff();
        skillCharacterShower.TurnOn();
    }
    private void TurnOnSave() {
               
        TurnOff();
        save.TurnOn();
    } 
    private void TurnOnSetting() {
               
        TurnOff();
    }
    private void TurnOnManual() {
               
        TurnOff();
        tutorial.TurnOn();
    }
    private void QuitGame() {
        ScenceControler.Load("Title");
    }

    private void TurnOff() {
        inventory.TurnOff();
        skillCharacterShower.TurnOff();
        skillListShower.TurnOff();
        save.TurnOff();
        tutorial.TurnOff();
    }

    public void Click(int index = 0) {

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

    private void Awake() {
        base.Awake();
        UseSelectSound = false;
        UseClickSound = true;
    }

    private SettingCategoryButton Parse(InteractButton target) {
        if (target is not SettingCategoryButton result)
            throw new TypeMissMatched(target.gameObject, typeof(SettingCategoryButton));

        return result;
    }
}