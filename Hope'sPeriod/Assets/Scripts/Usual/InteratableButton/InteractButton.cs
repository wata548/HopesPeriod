using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;

public abstract class InteractButton: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler {

    public int Index { get; set; }
    public InteractButtonManager Manager { get; set; } = null;

    public bool IsMouseOn { get; private set; } = false;
    public bool IsOn { get; set; } = false;
    
    public void OnPointerEnter(PointerEventData eventData) {

        IsOn = true;
        Manager.SelectButton(Index);
        IsMouseOn = true;
    }

    public void OnPointerExit(PointerEventData eventData) {

        IsOn = false;
        Manager.SelectOut(this);
        if (Manager.Selecting == Index) {
            
            Manager.Selecting = -1;
            Manager.Before = -1;
        }
        IsMouseOn = false;
    }

    public void OnPointerMove(PointerEventData eventData) {
        if (!IsMouseOn)
            return;

        OnPointerEnter(null);
    }
    
    public abstract void Click();
}

public abstract class InteractButtonManager: MonoBehaviour {

    [SerializeField] protected List<InteractButton> buttons;

    public abstract bool Interactable { get; set; }
    public int Selecting { get; set; } = 0;
    public int Before { get; set; } = -1;

    private void UpdateState() {

        if (Before != -1) {

            if (buttons[Before].IsOn) {
                
                SelectOut(buttons[Before]);
            }
            
        }

        buttons[Selecting].IsOn = true;
        SelectIn(buttons[Selecting]);
        Before = Selecting;
    }

    public void SelectButton(int index) {
        if (Selecting == index)
            return;

        Selecting = index;
        UpdateState();
    }
    
    public void NextButton() {

        Debug.Log(Selecting);
        
        Selecting++;
        if (Selecting >= buttons.Count) 
            Selecting = 0;

        UpdateState();
    }

    public void PriviousButton() {
        
        Debug.Log(Selecting);
        
        Selecting--;
        if (Selecting < 0)
            Selecting = buttons.Count - 1;

        UpdateState();
    }

    public abstract void SelectIn(InteractButton target);
    public abstract void SelectOut(InteractButton target);
    
    private void Awake() {

        int index = 0;
        foreach (var button in buttons) {
            
            button.Manager = this;
            button.Index = index++;
        }
    }
}