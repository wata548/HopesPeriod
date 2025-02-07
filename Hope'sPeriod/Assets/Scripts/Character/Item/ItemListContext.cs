using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ItemListContext: MonoBehaviour {

    //==================================================||Serialize 
    [SerializeField] private TMP_Text page;
    [SerializeField] private ItemListButtonManager itemListManager; 
    
    
    //==================================================||Field 
    public const int ButtonCount = 4;
    private bool needFirstItemInfoUpdate = true;
    private int currentPage = 1;
    private int maximumPage = 1;
    private List<ItemListButton> itemButtons = new();
    
    //==================================================||Property 

    //==================================================||Method 
   
    public void TurnOn() {
        currentPage = 1;
        needFirstItemInfoUpdate = true;
    }

    #region  Initializer

    private void SetItemButton() {
   
        itemButtons.Clear();
        for (int i = 1; i <= ButtonCount; i++) {
            itemButtons.Add(GameObject.Find($"Item{i}").GetComponent<ItemListButton>());
        }
    } 
    private void SetPageCount() {

        if (!ItemInfo.CheckTable())
            return;
        
        int temp = Inventory.Kind(CodeType.Item);
        maximumPage = temp / 4 + (temp % 4 != 0 ? 1 : 0);

        if (maximumPage <= 0) 
            maximumPage = 1;

        page.text = $"<{currentPage}/{maximumPage}>";
    }

    #endregion
    private void UpdatePageCount() {
        page.text = $"<{currentPage}/{maximumPage}>";
    }
    private bool UpdateItemList() {

        if (!ItemInfo.CheckTable())
            return false;

        int buttonIndex = 0;
        int itemIndex = 0;
        foreach (var item in Inventory.Items) {

            //check item
            if (item.Key.ToCodeType()!= CodeType.Item)
                continue;
            
            //check count
            if (item.Value <= 0)
                continue;
            
            itemIndex++;

            //apply show item
            if (itemIndex > ButtonCount * (currentPage - 1)) {
                itemButtons[buttonIndex].SetCode(item.Key);
                buttonIndex++;
            }

            if (buttonIndex >= ButtonCount)
                break;
        }

        //overflow data process(show nothing)
        for (int i = buttonIndex; i < ButtonCount; i++) {
            itemButtons[i].NoneCode();
        }

        return true;
    }

    #region PageControle

    public void NextPage() {
        
        currentPage++;
        if (currentPage > maximumPage) currentPage = 1;
        UpdatePageCount();
        UpdateItemList();
        //cursor index correct check
        itemListManager.CheckCursorIndex();
    }
    public void PriviousPage() {

        currentPage--;
        if (currentPage <= 0) currentPage = maximumPage;
        UpdatePageCount();
        UpdateItemList();
        //cursor index correct check
        itemListManager.CheckCursorIndex();
    }
    
    #endregion
    
   //==================================================||UnityFunc 
    private void Awake() {

        ItemInfo.SetTable();
        SetItemButton();
    }
    private void Update() {

        //First update
        if (needFirstItemInfoUpdate && UpdateItemList()) {
            SetPageCount();
            needFirstItemInfoUpdate = false;
        }
    }
}