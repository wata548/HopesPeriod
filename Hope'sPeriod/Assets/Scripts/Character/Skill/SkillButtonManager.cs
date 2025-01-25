using UnityEngine;
using System;

public class SkillButtonManager: InteractButtonManager {
    
    //==================================================||Set Inspector 
    
    [SerializeField] private GameObject skillList;
    [SerializeField] private Cursor cursor;
    
    //==================================================||Field 

    private RectTransform rect;    
    private static FloatingItemInfo floating = null;
    private readonly Vector2 defaultPos = new(0, 70);
    private readonly Vector2 interval = new(633, 0);
    
    //==================================================||Property 
    public static SkillButtonManager Instance { get; private set; } = null;
    public override bool Interactable { get; protected set; } = false;
    public int CharacterIndex { get; private set; } = 0;

    //==================================================||Method 
    
    public static void SetFloating(FloatingItemInfo floating) {
        SkillButtonManager.floating = floating;
    }
    
    public void TurnOn() {
        skillList.SetActive(true);
        Interactable = true;

        Refresh();
    }

    public void NextSelect() {
        CharacterIndex++;
        Refresh();
    }
    
    public void Refresh() {

        rect.localPosition = defaultPos + CharacterIndex * interval;
        foreach (var button in buttons) {

            Parse(button).Refresh();
        }
    }
    
    public void TurnOff() {
        skillList.SetActive(false);
        floating.TurnOff();
        Interactable = false;
        InitSelect();
        
        foreach (var button in buttons) {

            Parse(button).TurnOff();
        }
    }

    #region Interaction

    public override void SelectIn(InteractButton target) {
        
        cursor.SetIndex(Selecting);
    }
    public override void SelectOut(InteractButton target) {}
    
    #endregion

    private void Input() {
            
        if (InputManager.Instance.ClickAndHold(KeyTypes.Down)) {
            NextButton();
            UpdateState();
            
            cursor.SetIndex(Selecting);
        }
            
        if (InputManager.Instance.ClickAndHold(KeyTypes.Up)) {

            PriviousButton();
            UpdateState();
            
            cursor.SetIndex(Selecting);
        }
            
        if (InputManager.Instance.Click(KeyTypes.Select)) {
            
            int index = Selecting;
                
            Parse(buttons[index]).Click();
        }
    }
    
    private SkillButton Parse(InteractButton button) {
        if (button is not SkillButton itemButton) {
            throw new TypeMissMatched(button.gameObject, typeof(SkillButton));
        }
    
        return itemButton;
    }
    
    //==================================================||Unity Func 
    private void Start() {
        TurnOff();
    }

    private void Awake() {
        SkillInfo.SetTable();
        
        base.Awake();

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        rect = skillList.GetComponent<RectTransform>();
    }

    private void Update() {

        if (!Interactable)
            return;

        Input();
    }

   
}