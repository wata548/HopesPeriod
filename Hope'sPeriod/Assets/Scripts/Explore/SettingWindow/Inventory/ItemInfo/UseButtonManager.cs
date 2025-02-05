using System;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
public class UseButtonManager: InteractButtonManager {

    [Header("info Shower")]
    [SerializeField] private Image symbol;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemTag;
    [SerializeField] private TMP_Text itemDescription;
    [Header("buttonCursor")]
    [SerializeField] private Cursor cursor;

    [SerializeField] private InventoryButtonManager itemListManager;
    
    [SerializeField] private InventoryTargetButtonManager targetButtonManager;
    public InventoryTargetButtonManager TargetButtonManager => targetButtonManager;
    
    private bool keyBoardControlable = false;
    public bool KeyBoardControlable => keyBoardControlable;
    public void SetKeBoardControlable(bool target) => keyBoardControlable = target;  
      
    public override bool Interactable { get; protected set; } = true;
    private static readonly Color Active = Color.red;
    private static readonly Color Disactive = Color.white;
    private static readonly Color Disusable = Color.gray;
    public int Code { get; private set; } = 0;

    public void TurnOn() {
        Code = 0;
        SetEmpty();
        keyBoardControlable = false;
        cursor.SetIndex(0);
        Interactable = true;
    }

    public void TurnOff() {
        Interactable = false;
    }
    
    public void SetInfo(int code) {
   
        if (code == 0) {
            SetEmpty();
            return;
        }

        Code = code;
        symbol.color = Color.white;
        symbol.sprite = CodeInfo.LoadImage(code);
        itemName.text = ItemInfo.Name(code);
        itemTag.text =  $"x{Inventory.Count(code)}\n {ItemInfo.SimpleTag(code)}";
        itemDescription.text = ItemInfo.Description(code);

        cursor.gameObject.SetActive(true);
        if (code.ToCodeType() == CodeType.Item) {
            foreach (var button in buttons) {
                button.gameObject.SetActive(true);
            }

            Parse(buttons[0]).Text.color = Disactive;
        }
    }

    public void UpdateInfo() {
        if (Code == 0) {
            SetEmpty();
            return;
        }
        
        symbol.sprite = CodeInfo.LoadImage(Code);
        itemName.text = ItemInfo.Name(Code);
        itemTag.text =  $"x{Inventory.Count(Code)}\n {ItemInfo.SimpleTag(Code)}";
        itemDescription.text = ItemInfo.Description(Code);
        
        cursor.gameObject.SetActive(true);
        if (Code.ToCodeType() == CodeType.Item) {
            foreach (var button in buttons) {
                button.gameObject.SetActive(true);
            }

            if (Inventory.Count(Code) <= 0) {
                
                Parse(buttons[0]).Text.color = Disusable;
                itemListManager.ItemListRefresh(true);
            } 
                
            else 
                Parse(buttons[0]).Text.color = Disactive;
        }
    }
    
    public void SetEmpty() {
        symbol.color = new(0, 0, 0, 0);
        itemName.text = "";
        itemTag.text = "";
        itemDescription.text = "";

        foreach (var button in buttons) {
            button.gameObject.SetActive(false);
        }

        cursor.gameObject.SetActive(false);
    }

    private static int before = -1;
    public override void SelectIn(InteractButton target) {

        if (before != -1) {
            if (before == 0 && Inventory.Count(Code) <= 0)
                Parse(buttons[before]).SetColor(Disusable);
            else
                Parse(buttons[before]).SetColor(Disactive);    
        }

        before = Selecting;
        
        cursor.SetIndex(Selecting);
        
        if (Inventory.Count(Code) <= 0)
            return;
        Parse(target).SetColor(Active);
    }

    public override void SelectOut(InteractButton target) {
        cursor.Disappear();
    }

    private static UseButton Parse(InteractButton target) {
        if (target is not UseButton result)
            throw new TypeMissMatched(target.gameObject, typeof(UseButton));

        return result;
    }

    private void Update() {
        if (!Interactable || !KeyBoardControlable) return;
        if (InputManager.Instance.ClickAndHold(KeyTypes.Left)) {

            
            SelectOut(buttons[Selecting]);
            PriviousButton();
            SelectIn(buttons[Selecting]);
        }

        if (InputManager.Instance.ClickAndHold(KeyTypes.Right)) {
            SelectOut(buttons[Selecting]);
            NextButton();
            SelectIn(buttons[Selecting]);
        }

        if (InputManager.Instance.Click(KeyTypes.Cancel))
            buttons[1].Click();
        if (InputManager.Instance.Click(KeyTypes.Select))
            buttons[Selecting].Click();
    }
}