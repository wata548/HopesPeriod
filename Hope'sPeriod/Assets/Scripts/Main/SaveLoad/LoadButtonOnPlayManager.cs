using UnityEngine;

public class LoadButtonOnPlayManager: InteractButtonManager {
    public override bool Interactable { get; protected set; }
    private static readonly Color active = Color.red;
    private static readonly Color disactive = Color.white;
    private static readonly Color emptySlot = Color.gray;
    [SerializeField] private Cursor cursor;
    [SerializeField] private GameObject title;
    
    public void TurnOn() {
        Interactable = true;
        foreach (var button in buttons) {
            Parse(button)?.SetUp();
            button?.gameObject.SetActive(true);
        }

        title.gameObject.SetActive(true);
        cursor.gameObject.SetActive(true);
    }

    public void TurnOff() {
        Interactable = false;
        buttons.ForEach(button => button?.gameObject.SetActive(false));
        
        title.gameObject.SetActive(false);
        cursor.gameObject.SetActive(false);
    }
    public override void SelectIn(InteractButton target) {
        
        var targetButton = Parse(target);
        targetButton.SetColor(active);
        cursor.SetIndex(target.Index);
    }

    public override void SelectOut(InteractButton target) {
        cursor.Disappear();
        var targetButton = Parse(target);
        
        targetButton.SetColor(disactive);
        if(!targetButton.On)
            targetButton.SetColor(emptySlot);
    }

    private LoadButton Parse(InteractButton button) {
        if (button is null) return null;
        
        if (button is not LoadButton result)
            throw new TypeMissMatched(button.gameObject, typeof(LoadButton));

        return result;
    }
}