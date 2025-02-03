using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class InventoryButtonManager : InteractButtonManager {
    
    private static readonly Color Active = Color.red; 
    private static readonly Color Disactive = Color.white;
    [SerializeField] private Cursor cursor;
    [SerializeField] private InventoryItemInfo infoShower;
    public InventoryItemInfo InfoShower => infoShower;
    private CodeType category = CodeType.Item;
    private List<InventoryButton> fixButtons;
    
    
    public override bool Interactable { get; protected set; } = true;

    public void ButtonActive(int index) => fixButtons[index].SetFrameColor(Active);
    public void ButtonDisactive(int index) => fixButtons[index].SetFrameColor(Disactive);
    
    public override void SelectIn(InteractButton target) {

        if (!fixButtons[target.Index].On)
            return;
        
        cursor.SetIndex(target.Index);
    }

    public void SetButtonCode() {

        Page.CountPage(category);
        var factors = Page.Factors();
        int factorSize = factors.Count();
        for (int i = 0, size = buttons.Count; i < size; i++) {

            int code = 0;
            if (i < factorSize) code = factors[i];
            
            fixButtons[i].SetCode(code);
        }
    }
    
    public override void SelectOut(InteractButton target) {

        cursor.Disappear();
    }

    private static InventoryButton Parse(InteractButton target) {

        if (target is not InventoryButton result)
            throw new TypeMissMatched(target.name, typeof(TypeMissMatched));

        return result;
    }

    public void Update() {
        
        if (Input.GetKeyDown(KeyCode.T)) {
            
            SetButtonCode();
        }
    }

    private void Awake() {
        base.Awake();

        fixButtons = buttons
            .Select(Parse)
            .ToList();

        infoShower.SetEmpty();
    }
}