using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class CategoryButtonManager: InteractButtonManager {

    [SerializeField] private InventoryButtonManager inventoryButton;
    public override bool Interactable { get; protected set; } = true;
    private static readonly Color Disactive = new(0.3f, 0.3f, 0.3f);
    private static readonly Color Active = Color.white;
    private int select = 0;

    private static readonly List<CodeType> buttonMatchCategory = new() {
        CodeType.Item,
        CodeType.Document,
        CodeType.Key
    };

    public void TurnOn() => Interactable = true;
    public void TurnOff() => Interactable = false;
    
    public override void SelectIn(InteractButton target) {}

    public override void SelectOut(InteractButton target) {}

    public void SetSelect(int select) {
        Parse(buttons[this.select]).SetColor(Disactive);
        this.select = select;
        Parse(buttons[select]).SetColor(Active);

        inventoryButton.SetCategory(buttonMatchCategory[select]);
    }
    
    private static CategoryButton Parse(InteractButton button) {

        if (button is not CategoryButton result)
            throw new TypeMissMatched(button.gameObject, typeof(CategoryButton));

        return result;
    }
}