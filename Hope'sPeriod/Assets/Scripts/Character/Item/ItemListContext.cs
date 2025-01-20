using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Build.BuildPipelineTasks;

public class ItemListContext: MonoBehaviour {

    public static ItemListContext Instance { get; private set; } = null;
    
    [SerializeField] private TMP_Text page;
    [SerializeField] private ItemListButtonManager itemListManager; 
        
    private bool needFirstItemInfoUpdate = true;
    private int currentPage = 1;
    private int maximumPage = 1;
    private const int ButtonCount = 4;
    private List<ItemListButton> itemButtons = new();

    public void SetPageCount() {

        if (!ItemInfo.CheckTable())
            return;
        
        int temp = Inventory.Kind(CodeType.Item);
        maximumPage = temp / 4 + (temp % 4 != 0 ? 1 : 0);

        if (maximumPage <= 0) 
            maximumPage = 1;

        page.text = $"<{currentPage}/{maximumPage}>";
    }

    private void UpdatePage() {
        page.text = $"<{currentPage}/{maximumPage}>";
    }

    private void SetItemButton() {

        itemButtons.Clear();
        for (int i = 1; i <= ButtonCount; i++) {
            itemButtons.Add(GameObject.Find($"Item{i}").GetComponent<ItemListButton>());
        }
    }

    public bool UpdateItemButton() {

        if (!ItemInfo.CheckTable())
            return false;

        int buttonIndex = 0;
        int itemIndex = 0;
        foreach (var item in Inventory.Items) {

            //check item
            if ((CodeType)(item.Key / ItemInfo.CodeDigit) != CodeType.Item)
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

    public void NextPage() {
        
        currentPage++;
        if (currentPage > maximumPage) currentPage = 1;
        UpdatePage();
        UpdateItemButton();
        //cursor index correct check
        itemListManager.CheckIndex();
    }

    public void PriviousPage() {

        currentPage--;
        if (currentPage <= 0) currentPage = maximumPage;
        UpdatePage();
        UpdateItemButton();
        //cursor index correct check
        itemListManager.CheckIndex();
    }
    
    private void Awake() {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
        
        ItemInfo.SetTable();
        SetItemButton();
    }

    private void Update() {

        //First update
        if (needFirstItemInfoUpdate && UpdateItemButton()) {
            SetPageCount();
            needFirstItemInfoUpdate = false;
        }
    }
}