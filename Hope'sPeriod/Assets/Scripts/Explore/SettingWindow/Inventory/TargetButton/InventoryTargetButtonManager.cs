using System;
using TMPro;
using UnityEngine;

public class InventoryTargetButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; } = true;
    [SerializeField] private Cursor cursor;
    [SerializeField] private UseButtonManager itemInfo;
    public UseButtonManager ItemInfo => itemInfo;
    [SerializeField] private InventoryButtonManager inventoryButtonManager;
    [SerializeField] private CategoryButtonManager categoryButtonManager;
    [SerializeField] private GameObject targetSelectMessage;
    
    public int Code { get; private set; }

    public void TurnOn(int code) {

        Selecting = 0;
        
        cursor.TurnOn();
        targetSelectMessage.SetActive(true);
        cursor.gameObject.SetActive(true);
        Code = code;
        Interactable = true;

        itemInfo.SetInteractable(false);
        inventoryButtonManager.SetInteractable(false);
        categoryButtonManager.SetInteractable(false);
    }

    public void TurnOff() {
        targetSelectMessage.SetActive(false);
        cursor.gameObject.SetActive(false);
        Interactable = false;
        itemInfo.SetInteractable(true);
        inventoryButtonManager.SetInteractable(true);
        categoryButtonManager.SetInteractable(true);
    }
    
    public override void SelectIn(InteractButton target) {
        cursor.SetIndex(target.Index);
    }

    public override void SelectOut(InteractButton target) { }

    private void Update() {
        if (!Interactable)
            return;

        if (InputManager.Instance.ClickAndHold(KeyTypes.Left)) {
            var temp = Selecting;
            PriviousButton();
            if (buttons[Selecting].gameObject.gameObject.activeSelf)
                SelectIn(buttons[Selecting]);
            else
                Selecting = temp;
        }

        if (InputManager.Instance.ClickAndHold(KeyTypes.Right)) {
            var temp = Selecting;
            NextButton();
            if (buttons[Selecting].gameObject.gameObject.activeSelf)
                SelectIn(buttons[Selecting]);
            else
                Selecting = temp;
        }

        if (InputManager.Instance.Click(KeyTypes.Select)) {
            Parse(buttons[Selecting]).Click();
        }
        else if (InputManager.Instance.Click(KeyTypes.Cancel))
            TurnOff();
    }

    private InventoryTargetButton Parse(InteractButton button) {
        if (button is not InventoryTargetButton result)
            throw new TypeMissMatched(button.gameObject, typeof(InventoryTargetButton));

        return result;
    }
}