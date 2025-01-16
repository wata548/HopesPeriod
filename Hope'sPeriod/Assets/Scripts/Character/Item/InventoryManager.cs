using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class InventoryManager: MonoBehaviour {

    [SerializeField] private TMP_Text page;
    private bool needUpdate = true;
    private int currentPage = 1;
    private int maximumPage = 1;
    private const int ButtonCount = 4;
    private List<ItemButton> itemButtons = new();
    
    private void SetPageCount() {
        
        if (needUpdate && ItemInfo.CheckTable()) {
            needUpdate = false;
        
            int temp = Inventory.Kind(CodeType.Item);
            maximumPage = temp / 4 + (temp % 4 != 0 ? 1 : 0);

            if (maximumPage <= 0) 
                maximumPage = 1;

            page.text = $"<{currentPage}/{maximumPage}>";
        }
        
    }

    private void SetItemButton() {

        itemButtons.Clear();
        for (int i = 1; i <= ButtonCount; i++) {
            itemButtons.Add(GameObject.Find($"Item{i}").GetComponent<ItemButton>());
        }
    }

    private void UpdateItemButton() {

        int index = 0;
        int count = 0;
        foreach (var item in Inventory.Items) {

            if ((CodeType)(item.Key / ItemInfo.CodeDigit) != CodeType.Item)
                continue;
            
            if (item.Value <= 0)
                continue;
            
            count++;

            if (count > ButtonCount * (currentPage - 1)) {
                itemButtons[index].SetCode(item.Key);
                index++;
            }

            if (index >= ButtonCount)
                break;
        }

        Debug.Log(index);
        for (int i = index; i < ButtonCount; i++) {
            itemButtons[i].NoneCode();
        }
    }

    public void NextPage() {
        
        currentPage++;
        if (currentPage >= maximumPage) currentPage = 1;
        UpdateItemButton();
    }

    public void PriviousPage() {

        currentPage--;
        if (currentPage <= 0) currentPage = maximumPage;
        UpdateItemButton();
    }
    
    private void Awake() {
        ItemInfo.SetTable();
        SetItemButton();
    }

    private void Update() {

        SetPageCount();

        if (Input.GetKeyDown(KeyCode.A)) {
            NextPage();
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            PriviousPage();
        }
    }
}