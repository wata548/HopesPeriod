using System;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemButton: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    //If you set one component's tempItemInfo, All component share this tempItemInfo 
    [SerializeField] private FloatingItemInfo tempFloatingInfo = null; 
    [SerializeField]private Image image;
    private static FloatingItemInfo floatingInfo = null;
    private TMP_Text textInfo;
    private RectTransform rect;
    private int code = -1;
    
    private bool onMouse = false;
    private float startTime;
    private bool floatingOn = false;
    private bool needUpdate = false;
    private bool show = true;
    
    private const float AppearTime = 0.3f;

    private void RefreshFloating() {
        if (onMouse) {
            Debug.Log("refresh");
            startTime = Time.time;
            floatingOn = false;
            floatingInfo.TurnOff();
        }
    }
    
    public void SetCode(int code) {

        RefreshFloating();
        
        image.DOFade(1, 0);

        show = true;
        this.code = code;
        needUpdate = true;
    }

    public void NoneCode() {

        RefreshFloating();
        
        show = false;
        image.DOFade(0,0);
        textInfo.text = "";
    }
    
    public void OnPointerEnter(PointerEventData eventData) {

        startTime = Time.time;
        onMouse = true;
    }

    public void OnPointerExit(PointerEventData eventData) {

        floatingInfo.TurnOff();
        floatingOn = false;
        onMouse = false;
    }

    private void Awake() {

        if (floatingInfo is null && tempFloatingInfo is not null)
            floatingInfo = tempFloatingInfo;

        textInfo = GetComponentInChildren<TMP_Text>();
        rect = GetComponent<RectTransform>();
    }

    private void Update() {

        if (floatingInfo is null)
            throw new Exception($"please set floatingInfo in {gameObject.name}");

        if (code != -1 && needUpdate) {

            if(ItemInfo.CheckTable()) needUpdate = false;
            textInfo.text = $"{ItemInfo.Name(code)}\n{$"X{Inventory.Count(code)}".SetSize(1.3f)}";
        }
        
        if (!onMouse)
            return;
        
        if (show && !floatingOn && Time.time - startTime >= AppearTime) {
            floatingInfo.TurnOn();
            floatingOn = true;
        }

        floatingInfo.UpdatePivot(rect.position);
        floatingInfo.UpdatePosition();
        floatingInfo.UpdateInfo(code);
    }

    public void Click() {
        if(!show) return;

        if (ItemInfo.NeedSelect(code)) {
            
            if (ItemInfo.Revive(code)) {
                
            }
            else {
                
            }
        }
        Inventory.UseItem(code);
    }
}