using System;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemListButton: InteractButtonUI, IPointerEnterHandler, IPointerExitHandler {

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
    public bool Show { get; private set; } = true;
    
    private const float AppearTime = 0.3f;

    public void TurnOff() {
        onMouse = false;
        floatingOn = false;
    }
    
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

        Show = true;
        this.code = code;
        needUpdate = true;
    }

    public void NoneCode() {

        RefreshFloating();
        
        Show = false;
        image.DOFade(0,0);
        textInfo.text = "";
    }
    
   
    public override void OnPointerEnterExtra() {

        startTime = Time.time;
        onMouse = true;
    }

    public override void OnPointerExitExtra() {

        floatingInfo.TurnOff();
        floatingOn = false;
        onMouse = false;
    }
    

    private void Awake() {

        if (floatingInfo is null && tempFloatingInfo is not null) {
            
            floatingInfo = tempFloatingInfo;
            ItemListButtonManager.SetFloating(tempFloatingInfo);
        }

        textInfo = GetComponentInChildren<TMP_Text>();
        rect = GetComponent<RectTransform>();
    }

    private void Update() {

        if (floatingInfo is null)
            throw new Exception($"please set floatingInfo in {gameObject.name}");

        if (!Manager.Interactable)
            return;
        
        //firstUpdate
        if (code != -1 && needUpdate) {

            if(ItemInfo.CheckTable()) needUpdate = false;
            textInfo.text = $"{ItemInfo.Name(code)}\n{$"X{Inventory.Count(code)}".SetSize(1.3f)}";
        }
        
        if (!onMouse)
            return;
        
        if (Show && !floatingOn && Time.time - startTime >= AppearTime) {
            floatingInfo.TurnOn();
            floatingOn = true;
        }

        floatingInfo.UpdatePivot(rect.position);
        floatingInfo.UpdatePosition();
        floatingInfo.UpdateInfo(code);
    }


    public override void Click() {
        if (!Show) return;
        if (!Manager.Interactable) return;

        if (!ItemInfo.NeedSelect(code)) {

            Inventory.UseItem(code);
           
            return;
        }

        TargetButtonManager.Instance.TurnOn(SelectType.Players);
        InteractableOff();
    }

    public void InteractableOff() {
        floatingInfo.TurnOff();
        floatingOn = false;
        Manager.Interactable = false;
        onMouse = false;
    }

    public void InteractableOn() {
        Manager.Interactable = true;
    }
}