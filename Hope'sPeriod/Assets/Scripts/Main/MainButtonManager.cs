using System;
using System.Security;
using UnityEngine.UI;

public class MainButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; } = true;

    private int tempIndex = -1;
    public override void SelectIn(InteractButton target) {
        if (tempIndex != -1)
            SelectOut(buttons[tempIndex]);
        
        Parse(target).Active();
        AudioManager.Instance.PlayOne(FmodEvents.Instance.SelectIn);

        tempIndex = Selecting;
    }
    
    public override void SelectOut(InteractButton target) {
        Parse(target).Disactive();
    }

    public void Init() {
        foreach (var button in buttons) {
            SelectOut(button);
        }

        tempIndex = -1;
        Selecting = 0;
        SelectIn(buttons[0]);
    }

    private MainButton Parse(InteractButton button) {
        if (button is not MainButton result)
            throw new TypeMissMatched(button.gameObject, typeof(MainButton));

        return result;
    }

    private void Awake() {
        base.Awake();
        Selecting = 0;
        SelectIn(buttons[0]);
    }
    
    private void Update() { 
        if(!Interactable)
            return;

        if (InputManager.Instance.ClickAndHold(KeyTypes.Up)) {
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
        else if (InputManager.Instance.Click(KeyTypes.Interaction)) {
            buttons[Selecting].gameObject
                .GetComponent<Button>()
                .onClick?.Invoke();
        }
    }
}