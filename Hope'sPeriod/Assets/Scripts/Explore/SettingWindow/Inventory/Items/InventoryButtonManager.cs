using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using VInspector.Libs;

public class InventoryButtonManager : InteractButtonManager {
    
    private static readonly Color Active = Color.red; 
    private static readonly Color Disactive = Color.white;
    private const int ButtonCount = 15;
    private const int NewLinePoint = 5;
    [SerializeField] private Cursor cursor;
    [SerializeField] private UseButtonManager infoShower;
    [SerializeField] private TMP_Text pageInfo;
    private CodeType category = CodeType.Item;
    private List<InventoryButton> fixButtons;

    public UseButtonManager InfoShower => infoShower;
    public override bool Interactable { get; protected set; } = true;

    public void TurnOn() {
        Interactable = true;
        category = CodeType.Item;
        ItemListRefresh();
        cursor.Disappear();
    }

    public void TurnOff() => Interactable = false;
    
    
    public void ButtonActive(int index) => fixButtons[index].SetFrameColor(Active);

    public void ButtonDisactive(int index) {
        bool isOn = fixButtons[index].On;
        if(isOn)
            fixButtons[index].SetFrameColor(Disactive);
        else 
            fixButtons[index].SetFrameColor(new (0,0,0,0));
    }
    
    
    public override void SelectIn(InteractButton target) {

        if (!fixButtons[target.Index].On)
            return;
        
        cursor.SetIndex(target.Index);
    }

    public void ItemListRefresh(bool selectCancel = false) {

        Page.CountPage(category);
        if(Page.MaxPage == 0)
            pageInfo.text = "<0 / 0>";
        else 
            pageInfo.text = $"<{Page.CurrentPage + 1} / {Page.MaxPage}>";
        
        var factors = Page.Factors();
        int factorSize = factors.Count();
        for (int i = 0, size = buttons.Count; i < size; i++) {

            int code = 0;
            if (i < factorSize) code = factors[i];
            
            fixButtons[i].SetCode(code);
        }

        if (selectCancel)
            fixButtons[0].SelectCancel();
    }

    public void SetCategory(CodeType category) {
        
        this.category = category;
        ItemListRefresh();
    }
    
    public override void SelectOut(InteractButton target) {

        cursor.Disappear();
    }

    private static InventoryButton Parse(InteractButton target) {

        if (target is not InventoryButton result)
            throw new TypeMissMatched(target.name, typeof(TypeMissMatched));

        return result;
    }

    public void Update() {

        bool keyBoardContorlable = !infoShower.KeyBoardControlable;
        if (!Interactable || !keyBoardContorlable)
            return;


        bool input = false;
        bool controlable = Page.MaxPage > 0;
        if (InputManager.Instance.ClickAndHold(KeyTypes.Right) && controlable) {

            bool empty = (Selecting + 1 != ButtonCount && !fixButtons[Selecting + 1].On);
            if ((Selecting + 1) / NewLinePoint != Selecting / NewLinePoint || empty) {

                NextPage();
                Selecting = Selecting / NewLinePoint * NewLinePoint;
                while (!fixButtons[Selecting].On) Selecting -= NewLinePoint;
            }
            else Selecting++;

            input = true;
        }
        if (InputManager.Instance.ClickAndHold(KeyTypes.Left) && controlable) {
            if (Selecting == -1) Selecting = 0;
            else if (Selecting % NewLinePoint == 0) {
                PriviousPage();
                Selecting += NewLinePoint - 1;
                while (!fixButtons[Selecting].On) Selecting--;
            }
            else Selecting--;

            input = true;
        }

        if (InputManager.Instance.ClickAndHold(KeyTypes.Up) && controlable) {
            Selecting -= NewLinePoint;
            if (Selecting < 0) Selecting += ButtonCount;
            while (!fixButtons[Selecting].On) Selecting -= NewLinePoint;
            input = true;
        }

        if (InputManager.Instance.ClickAndHold(KeyTypes.Down) && controlable) {
            Selecting += NewLinePoint;
            if (Selecting >= ButtonCount) Selecting -= ButtonCount;
            if (!fixButtons[Selecting].On) Selecting %= NewLinePoint;
            input = true;
        }

        if (InputManager.Instance.Click(KeyTypes.Select) && Selecting != -1) {
            buttons[Selecting].Click();
            if(category == CodeType.Item)
                InfoShower.SetKeBoardControlable(true);
        }

        if (input) cursor.SetIndex(Selecting);
    }

    public void NextPage() {
        if(!Interactable) return;
        
        if (Page.MaxPage <= 1) return;
        Page.NextPage();
        ItemListRefresh();
    }

    public void PriviousPage() {
        if(!Interactable) return;
        
        if (Page.MaxPage <= 1) return;
        Page.PriviousPage();
        ItemListRefresh();
    }

    private void Awake() {
        base.Awake();

        fixButtons = buttons
            .Select(Parse)
            .ToList();
    }
}