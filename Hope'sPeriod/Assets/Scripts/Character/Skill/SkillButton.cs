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

    public int Code { get; private set; } = 0;
    
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

        bool interactable = Manager.Interactable;
        bool setTable = SkillInfo.CheckTable();
        if (!interactable || !setTable)
            return;
        
        //firstUpdate
        if (needUpdate) {

            needUpdate = false;
            int characterIndex = Parse(Manager).CharacterIndex;
            Code = ControleCharacterInfo.Instance.GetSkill(characterIndex, Index);

            Show = true;
            if (Code == 0) {

                textInfo.text = "";
                Show = false;
            }
            else {
                textInfo.text = $"{SkillInfo.Name(Code)}\n{SkillInfo.SimpleTag(Code).SetSize(1.15f)}";

                if (SkillInfo.Useable(characterIndex, Code))
                    textInfo.AddColor(Usable);
                else
                    textInfo.AddColor(Unusable);
            }
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

        if (SkillInfo.NeedSelect(Code, out int itemCode)) {

            TargetButtonManager.Instance.TurnOn(Code);
            Manager.SetInteractable(false);
            return;
        }
        Parse(Manager).NextSelect();
    }

    private static SkillButtonManager Parse(InteractButtonManager manager) {
        
        if (manager is not SkillButtonManager skillManager)
            throw new TypeMissMatched(manager.gameObject, typeof(SkillButtonManager)); 
        
        return skillManager;
    }
}