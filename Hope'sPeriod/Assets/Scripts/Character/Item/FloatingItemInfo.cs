using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingItemInfo : MonoBehaviour
{
    
   //==================================================||Set Inspector 
   
    [SerializeField]private GameObject floating;
    
   //==================================================||Field 
    private RectTransform rect;
    private Vector2 screenSize;
    private TMP_Text info;

    private enum ShowDirection {
        DownLeft = 0b00,
        DownRight = 0b01,
        UpLeft = 0b10, 
        UpRight = 0b11
    }

    private readonly Dictionary<ShowDirection, Vector2> pivotMatch = new() {
        {ShowDirection.UpLeft, Vector2.zero},
        {ShowDirection.UpRight, Vector2.right},
        {ShowDirection.DownLeft, Vector2.up},
        {ShowDirection.DownRight, Vector2.one}
    };

   //==================================================||Method 
    public void TurnOn() {
        floating.SetActive(true);
    }
    
    public void TurnOff() {
        floating.SetActive(false);
    }
    
    public void UpdateInfo(int code) {

        info.text = ItemInfo.Description(code);
    }
    
    public void UpdatePivot(Vector2 pos) {
        
        Vector2 mousePos = Input.mousePosition;
        ShowDirection result = ShowDirection.DownLeft;
        
        if (mousePos.y > pos.y) {
            result |= ShowDirection.UpLeft;
        }
        if (mousePos.x < pos.x) {
            result |= ShowDirection.DownRight;
        }
        rect.pivot = pivotMatch[result];
    } 
    
    public void UpdatePosition() {
        
        Vector2 mousePos = Input.mousePosition;
        rect.position = mousePos;
    }
    
   //==================================================||Unity Func 
    private void Awake() {
    
        TurnOff();
        info = floating.GetComponentInChildren<TMP_Text>();
        screenSize = new(Screen.width, Screen.height);
        rect = floating.GetComponent<RectTransform>();
    }
}
