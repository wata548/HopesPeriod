using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtonManager : InteractButtonManager {
    
    private static readonly Color Active = Color.red; 
    private static readonly Color Disactive = Color.white;
    [SerializeField] private Cursor cursor;

    public override bool Interactable { get; protected set; } = true;
    public override void SelectIn(InteractButton target) {

        Parse(target).SetFrameColor(Active);
        cursor.SetIndex(target.Index);
    }

    public override void SelectOut(InteractButton target) {
        
        Parse(target).SetFrameColor(Disactive);
    }

    private static InventoryButton Parse(InteractButton target) {

        if (target is not InventoryButton result)
            throw new TypeMissMatched(target.name, typeof(TypeMissMatched));

        return result;
    }
}