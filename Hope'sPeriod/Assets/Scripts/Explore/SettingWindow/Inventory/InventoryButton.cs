using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryButton: InteractButtonUI {

    [SerializeField] private Image icon = null;
    private Image frame;
    public int Code { get; private set; } = 0;

    public bool On { get; private set; } = false;
    private static int beforeIndex = -1;

    public override void Click() {

        if (!On) return;
        
        var manager = Parse(Manager);
        if (beforeIndex != -1) {
            manager.ButtonDisactive(beforeIndex);
        } 
        beforeIndex = Index;
        manager.ButtonActive(Index);
        manager.InfoShower.SetInfo(Code);
    }

    public void SetCode(int code) {

        Code = code; 
        if (code == 0) {
            On = false;
            icon.color = new(0, 0, 0, 0);
            frame.color = new(0, 0, 0, 0);
        }
        else {
            On = true;
            icon.sprite = CodeInfo.LoadImage(Code);
            icon.color = Color.white;
            frame.color = Color.white;
        }
    }
    
    private void Awake() {
        
        frame = GetComponent<Image>();
    }

    public void SetFrameColor(Color color) => frame.color = color;

    public static InventoryButtonManager Parse(InteractButtonManager manager) {
        if (manager is not InventoryButtonManager inventoryManager)
            throw new TypeMissMatched(manager.gameObject, typeof(InventoryButtonManager));

        return inventoryManager;
    }
}

