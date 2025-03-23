using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TargetButtonManager : InteractButtonManager {
    
    
    //==================================================||Property 
   
    public int Code { get; private set; }
    public override bool Interactable { get; protected set; } = false;
    public static TargetButtonManager Instance { get; private set; } = null;
    
    //==================================================||Method 
    
    public void TurnOn(int code) {

        Debug.Log(code);
        
        this.Code = code;
        Interactable = true;
            
        SkillButtonManager.Instance.SetInteractable(false);
        SelectCursor.Instance.TurnOn();
    }

    public void TurnOff() {

        int targetIndex = SelectCursor.Instance.Index;
        var target = CharactersInfoBattle.Instance.CharacterInfos[targetIndex].Info;
        if (Code.ToCodeType() == CodeType.Item && !ItemInfo.UseAble(Code, target)) {
            return;
        }

        if (Code.ToCodeType() == CodeType.Skill && !SkillInfo.Useable(Code, target)) {
            return;
        }
        
        Interactable = false;
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
                
         if (InputManager.Instance.ClickAndHold(KeyTypes.Right)) {
             SelectCursor.Instance.AddIndex();
         }
        
         if (InputManager.Instance.ClickAndHold(KeyTypes.Left)) {
             SelectCursor.Instance.ExtractIndex();
         }
        
         if (InputManager.Instance.Click(KeyTypes.Select)) {
             TurnOff();
         }
                
         else if (InputManager.Instance.Click(KeyTypes.Cancel)) {
             
             SelectCursor.Instance.TurnOff();
             Interactable = false;

             if (GameFSM.Instance.PlayerTurnState == PlayerTurnState.Item) {
                 
                 ItemListButtonManager.Instance.SetInteractable(true);
             }
             else if (GameFSM.Instance.PlayerTurnState == PlayerTurnState.Attack) {

                 SkillButtonManager.Instance.SetInteractable(true);
             }
         }
    }
}