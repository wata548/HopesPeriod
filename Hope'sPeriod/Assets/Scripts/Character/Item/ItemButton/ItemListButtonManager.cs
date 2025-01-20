using System;
using Unity.VisualScripting;
using UnityEditor.AddressableAssets.Build.BuildPipelineTasks;
using UnityEngine;
using VInspector.Libs;

public class ItemListButtonManager: InteractButtonManager {
    public override bool Interactable { get; set; } = false;
    
    [SerializeField] private GameObject itemList;

    public static ItemListButtonManager Instance { get; private set; } = null;
    
    public void TurnOn() {
        itemList.SetActive(true);
        Interactable = true;
    }

    public void TurnOff() {
        itemList.SetActive(false);
        Interactable = false;
    }
    
    public override void SelectIn(InteractButton target) {

        if (target is not ItemListButton button) {
            throw new Exception($"This Interactable Button type didn't match to ItemListButton.\nThis Interactable button type must be ItemListButton\n(ObjectName : {target.gameObject.name}");
        }
        
        if (!button.Show)
            return;
        
        ItemListCursor.Instance.SetIndex(target.Index);
    }

    public override void SelectOut(InteractButton target) {}

    public void CheckIndex() {
        if (Selecting == -1)
            return;

        if (buttons[Selecting] is not ItemListButton button) {
            throw new Exception($"This Interactable Button type didn't match to ItemListButton.\nThis Interactable button type must be ItemListButton\n(ObjectName : {buttons[Selecting].gameObject.name}");
        }

        if (!button.Show) {

            Debug.Log("init");
            SelectButton(0);
        }

    }

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
        
        if (InputManager.Instance.Click(KeyTypes.Right)) {
            ItemListContext.Instance.NextPage();
        }

        if (InputManager.Instance.Click(KeyTypes.Left)) {
            ItemListContext.Instance.PriviousPage();
        }
    }
}