using System;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using TMP_Text = TMPro.TMP_Text;
using DG.Tweening;
using SpreadInfo;

public class SkillButton: InteractButtonUI {
    
    //If you set one component's tempItemInfo, All component share this tempItemInfo 
    [SerializeField] private FloatingItemInfo tempFloatingInfo = null;
    private static FloatingItemInfo floatingInfo = null;
    private TMP_Text textInfo;
    private RectTransform rect;

    private int code = 0;
    
    private bool onMouse = false;
    private float startTime;
    private bool floatingOn = false;
    private bool needUpdate = false;
    
    private readonly Color Usable = Color.white;
    private readonly Color Unusable = Color.gray;
    
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
    
    public void Refresh() {

        RefreshFloating();

        Show = true;
        needUpdate = true;
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
            SkillButtonManager.SetFloating(tempFloatingInfo);
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
        if (needUpdate) {

            if (!SkillInfo.CheckTable())
                return;
            
            needUpdate = false;
            int characterIndex = ParseManager().CharacterIndex;
            code = ControleCharacterInfo.Instance.GetSkill(characterIndex, Index);

            Show = true;
            if (code == 0) {

                textInfo.text = "";
                Show = false;
            }
            else {
                textInfo.text = $"{SkillInfo.Name(code)}\n{$"{SkillInfo.SimpleDescription(code)}".SetSize(1.15f)}";

                if (SkillInfo.Useable(characterIndex, code))
                    textInfo.AddColor(Usable);
                else
                    textInfo.AddColor(Unusable);
            }
            
            
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

    public SkillButtonManager ParseManager() {
        if (Manager is not SkillButtonManager manager)
            throw new TypeMissMatched(Manager.gameObject, typeof(SkillButtonManager));

        return manager;
    }
}