using UnityEngine;
using UnityEngine.UI;

public class LoadButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; }
    private static readonly Color active = Color.red;
    private static readonly Color disactive = Color.white;
    private static readonly Color emptySlot = Color.gray;
    [SerializeField] private GameObject window;
    [SerializeField] private Cursor cursor;
    [SerializeField] private MainButtonManager main;
    [SerializeField] private bool isMain = true;
    
    public void TurnOn() {
        Interactable = true;
        window.SetActive(true);

        Selecting = 0;
    }

    public void TurnOff() {
        Interactable = false;
        window.SetActive(false);
        
        if(isMain)
            main.SetInteractable(true);
    }
    public override void SelectIn(InteractButton target) {
        
        var targetButton = Parse(target);
        if (targetButton.On) {
            
            targetButton.SetColor(active);
            cursor.SetIndex(target.Index);
        }
    }

    public override void SelectOut(InteractButton target) {
        cursor.Disappear();
        var targetButton = Parse(target);
        
        targetButton.SetColor(disactive);
        if(!targetButton.On)
            targetButton.SetColor(emptySlot);
    }

    private LoadButton Parse(InteractButton button) {
        if (button is not LoadButton result)
            throw new TypeMissMatched(button.gameObject, typeof(LoadButton));

        return result;
    }

    private void Start() {
        foreach (var button in buttons) {
            Parse(button).SetUp();
        }
        TurnOff();
    }

    private void Update() {
        if (!Interactable) 
            return;
               
        if (InputManager.Instance.Click(KeyTypes.Cancel)) {
            TurnOff();
        }
        else if (InputManager.Instance.Click(KeyTypes.Interaction)) {
            buttons[Selecting].GetComponent<Button>()
                .onClick?.Invoke();
        }
        else if (InputManager.Instance.ClickAndHold(KeyTypes.Up)) {
            
            if(Selecting != -1)
                SelectOut(buttons[Selecting]);
            PriviousButton();
            SelectIn(buttons[Selecting]);
        }
        else if (InputManager.Instance.ClickAndHold(KeyTypes.Down)) {
            if(Selecting != -1)
                SelectOut(buttons[Selecting]);
            NextButton();
            SelectIn(buttons[Selecting]);
        }
    }
}