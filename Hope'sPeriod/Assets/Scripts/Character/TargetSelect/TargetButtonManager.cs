using System;
using System.Security.Cryptography;

public class TargetButtonManager : InteractButtonManager {
    
    
    //==================================================||Property 
   
    public int Code { get; private set; }
    public override bool Interactable { get; set; } = false;
    public static TargetButtonManager Instance { get; private set; } = null;
    
    //==================================================||Method 
    
    public void TurnOn(SelectType type, int code) {
            
        this.Code = code;
        Interactable = true;
            
        SelectCursor.Instance.TurnOn(type);
    }

    public void TurnOff() {

        if (GameFSM.Instance.PlayerTurnState == PlayerTurnState.Item)
            GameFSM.Instance.AfterSetTarget();
            
        SelectCursor.Instance.TurnOff();
    }
    
    public override void SelectIn(InteractButton target) {
        SelectCursor.Instance.SetIndex(target.Index);
    }
    public override void SelectOut(InteractButton target) {}
    
    //==================================================||Unity Func 
   
    private void Awake() {
        base.Awake();
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    private void Update() {
         if (!Interactable)
             return;
                
         if (InputManager.Instance.Click(KeyTypes.Right) || InputManager.Instance.Hold(KeyTypes.Right)) {
             SelectCursor.Instance.AddIndex();
         }
        
         if (InputManager.Instance.Click(KeyTypes.Left) || InputManager.Instance.Hold(KeyTypes.Left)) {
             SelectCursor.Instance.ExtractIndex();
         }
        
         if (InputManager.Instance.Click(KeyTypes.Select)) {
             TurnOff();
         }
                
         else if (InputManager.Instance.Click(KeyTypes.Cancel)) {
             
             SelectCursor.Instance.TurnOff();
             Interactable = false;
             ItemListButtonManager.Instance.Interactable = true;
         }
    }
}