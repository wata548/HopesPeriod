using UnityEngine;
using UnityEngine.UI;

public class LoadButtonOnPlayManager: InteractButtonManager {
    public override bool Interactable { get; protected set; }
    private static readonly Color active = Color.red;
    private static readonly Color disactive = Color.white;
    private static readonly Color emptySlot = Color.gray;
    [SerializeField] private Cursor cursor;
    [SerializeField] protected GameObject title;
    
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
    private void Update() {
        if (!Interactable) 
            return;
                    
        if (InputManager.Instance.Click(KeyTypes.Interaction)) {
            buttons[Selecting].GetComponent<Button>()
                .onClick?.Invoke();
        }
        else if (InputManager.Instance.ClickAndHold(KeyTypes.Up)) {
                 
            if(Selecting != -1 && Selecting != 0)
                SelectOut(buttons[Selecting]);
            PriviousButton();
            
            if(Selecting == 0)
                PriviousButton();
            
            SelectIn(buttons[Selecting]);
        }
        else if (InputManager.Instance.ClickAndHold(KeyTypes.Down)) {
            if(Selecting != -1 && Selecting != 0)
                SelectOut(buttons[Selecting]);
            NextButton();
            
            if(Selecting == 0)
                NextButton();
            
            SelectIn(buttons[Selecting]);
        }
    }
}