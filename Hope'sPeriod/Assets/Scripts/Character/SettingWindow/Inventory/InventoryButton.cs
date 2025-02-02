using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton: InteractButtonUI {

    [SerializeField] private Image icon = null;
    private Image frame;
    public int Code { get; set; } = 0;
    public override void Click() { }

    private void Awake() {
        
        frame = GetComponent<Image>();
    }

    public void SetFrameColor(Color color) => frame.color = color;
}

