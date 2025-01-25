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

    protected bool isMouseOn = false;
    public bool IsOn { get; private set; } = false;

    
   //==================================================||Method 
    public void SetIsOn(bool isOn) => IsOn = isOn;
   
    public virtual void OnPointerEnterExtra() {}
    public virtual void OnPointerExitExtra() {}
    public virtual void OnPointerMoveExtra() {}
    
    public void OnPointerEnter(PointerEventData eventData) {

        if (!Manager.Interactable)
            return;
        
        IsOn = true;
        Manager.SelectButton(Index);
        isMouseOn = true;
        OnPointerEnterExtra();
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!Manager.Interactable)
            return;
        
        IsOn = false;
        Manager.SelectOut(this);
        if (Manager.Selecting == Index) {

            Manager.InitSelect();
            Manager.Before = -1;
        }
        isMouseOn = false;
        OnPointerExitExtra();
    }

    public void OnPointerMove(PointerEventData eventData) {
        
        if (!Manager.Interactable)
            return;
        
        if (!isMouseOn)
            return;

        OnPointerEnter(null);
        OnPointerMoveExtra();
    }
    
   //==================================================||Abstract Method 
    public abstract void Click();
}

/// <summary>
/// <para>
/// It use Unity Function: Awake
/// </para>
/// If you want to use Awake function, must include this
/// <code>private void Awake(){ base.Awake(); }</code>  
/// </summary>
public abstract class InteractButtonManager: MonoBehaviour {

    [SerializeField] protected List<InteractButton> buttons;

    //if it is false, you can't interact this
    public abstract bool Interactable { get; protected set; }
    public int Selecting { get; protected set; } = 0;
    
    public int Before { get; set; } = -1;

   //==================================================||Method 

   public void SetInteractable(bool interactable) => Interactable = interactable;
   
    protected void UpdateState() {

        if (!Interactable)
            return;
        
        if (Before != -1) {

            if (buttons[Before].IsOn) {
                
                SelectOut(buttons[Before]);
                buttons[Before].SetIsOn(false);
            }
            
        }

        buttons[Selecting].SetIsOn(true);
        SelectIn(buttons[Selecting]);
        Before = Selecting;
    }

    //It used by button script
    public void SelectButton(int index) {
        
        if (!Interactable)
            return;        
        
        if (Selecting == index)
            return;

        Selecting = index;

        UpdateState();
    }
    
    //Controle function(if you want to control by keyboard or etc., You should use this function)  
    public void NextButton() {
        if (!Interactable)
            return;
        
        Selecting++;
        if (Selecting >= buttons.Count) 
            Selecting = 0;

    }

    public void PriviousButton() {
        if (!Interactable)
            return;
        
        Selecting--;
        if (Selecting < 0)
            Selecting = buttons.Count - 1;

    }

    public void InitSelect() {
        
        //It makes able to interact mouse's shake movement
        Selecting = -1;
    }
    
   //==================================================||Abstract Function 
    public abstract void SelectIn(InteractButton target);
    public abstract void SelectOut(InteractButton target);
    
   //==================================================||Unity Function 
    protected void Awake() {

        int index = 0;
        foreach (var button in buttons) {
            
            button.Manager = this;
            button.Index = index++;
        }
    }
}