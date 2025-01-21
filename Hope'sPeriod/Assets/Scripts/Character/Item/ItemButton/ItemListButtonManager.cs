using System;
using Unity.VisualScripting;
using UnityEditor.AddressableAssets.Build.BuildPipelineTasks;
using UnityEngine;
using UnityEngine.UI;
using VInspector.Libs;

public class ItemListButtonManager: InteractButtonManager {
    
    //==================================================||Set Inspector 
    
    [SerializeField] private GameObject itemList;

    //==================================================||Field 
    private static FloatingItemInfo floating = null;
    
    //==================================================||Property 
    public static ItemListButtonManager Instance { get; private set; } = null;
    public override bool Interactable { get; set; } = false;

    //==================================================||Method 
    
    public static void SetFloating(FloatingItemInfo floating) {
        ItemListButtonManager.floating = floating;
    }
    
    public void TurnOn() {
        itemList.SetActive(true);
        ItemListContext.Instance.TurnOn();
        ItemListCursor.Instance.TurnOn();
        Interactable = true;
    }

    public void TurnOff() {
        itemList.SetActive(false);
        floating.TurnOff();
        Interactable = false;
        Selecting = -1;
        
        foreach (var button in buttons) {

            Parse(button).TurnOff();
        }
    }

    #region Interaction

    

    public override void SelectIn(InteractButton target) {

        var button = Parse(target);
        
        if (!button.Show)
            return;
        
        ItemListCursor.Instance.SetIndex(target.Index);
    }
    public override void SelectOut(InteractButton target) {}
    
    #endregion

    public void CheckCursorIndex() {

        int index = ItemListCursor.Instance.Index;
        var button = Parse(buttons[index]);
        if (!button.Show) {

            Debug.Log("init");
            SelectButton(0);
        }

    }

    private void Input() {
        if (InputManager.Instance.Click(KeyTypes.Right)) {
            ItemListContext.Instance.NextPage();
        }
            
        if (InputManager.Instance.Click(KeyTypes.Left)) {
            ItemListContext.Instance.PriviousPage();
        }
            
        if (InputManager.Instance.Click(KeyTypes.Down)) {
            int index = ItemListCursor.Instance.Index + 1;
            int limit = buttons.Count;
            
            if (index >= limit) {
                index = 0;
            }
    
            if (!Parse(buttons[index]).Show)
                index = 0;
                
            Selecting = -1;
            ItemListCursor.Instance.SetIndex(index);
        }
            
        if (InputManager.Instance.Click(KeyTypes.Up)) {
            int index = ItemListCursor.Instance.Index - 1;
            int limit = buttons.Count;
            
            if (index < 0) {
                index = limit - 1;
            }
    
            if (!Parse(buttons[index]).Show) {
    
                bool change = false;
                for (int i = buttons.Count - 1; i > -1; i--) {
                    if (!Parse(buttons[i]).Show) continue;
                        
                    change = true;
                    index = i;
                    break;
                }
    
                if (!change) index = 0;
            }
    
            Selecting = -1;
            ItemListCursor.Instance.SetIndex(index);
        }
            
        if (InputManager.Instance.Click(KeyTypes.Select)) {
            
            int index = ItemListCursor.Instance.Index;
                
            Parse(buttons[index]).Click();
        }
    }
    
    private ItemListButton Parse(InteractButton button) {
        if (button is not ItemListButton itemButton) {
            throw new TypeMissMatched(button.gameObject, typeof(ItemListButton));
        }
    
        return itemButton;
    }
    
    //==================================================||Unity Func 
    private void Start() {
        TurnOff();
    }

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

        Input();
    }

   
}