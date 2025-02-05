using System;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemListButton : InteractButtonUI {

    //If you set one component's tempItemInfo, All component share this tempItemInfo 
    [SerializeField] private FloatingItemInfo tempFloatingInfo = null;
    [SerializeField] private Image image;
    private static FloatingItemInfo floatingInfo = null;
    private TMP_Text textInfo;
    private RectTransform rect;
    public int Code { get; private set; }= -1;
    
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

        image.sprite = CodeInfo.LoadImage(code);
        image.DOFade(1, 0);

        Show = true;
        this.Code = code;
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

        bool tableSetting = ItemInfo.CheckTable();
        bool interactable = Manager.Interactable; 
        if (!tableSetting || !interactable)
            return;
        
        //Set name and count info
        if (Code != -1 && needUpdate) {

            needUpdate = false;
            textInfo.text = $"{ItemInfo.Name(Code)}\n{$"X{Inventory.Count(Code)}".SetSize(1.3f)}";
        }

        ControlFloating();
    }

    private void ControlFloating() {
        
        if (!onMouse)
            return;
                
        if (Show && !floatingOn && Time.time - startTime >= AppearTime) {
            floatingInfo.TurnOn();
            floatingOn = true;
        }
        
        floatingInfo.UpdatePivot(rect.position);
        floatingInfo.UpdatePosition();
        floatingInfo.UpdateInfo(Code);
    }

    public override void Click() {
        if (!Show) return;

        if (!ItemInfo.NeedSelect(Code)) {

            Inventory.UseItemBattle(Code);
           
            return;
        }

        TargetButtonManager.Instance.TurnOn(Code);
        InteractableOff();
    }

    public void InteractableOff() {
        floatingInfo.TurnOff();
        floatingOn = false;
        Manager.SetInteractable(false);
        onMouse = false;
    }

    public void InteractableOn() {
        Manager.SetInteractable(true);
    }
}