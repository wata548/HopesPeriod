using UnityEngine;
using System;
using Mono.Cecil.Cil;

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
        CharacterIndex = 0;
        
        Refresh();
    }

    public void NextSelect() {
        CharacterIndex++;

        var characterControler = ControleCharacterInfo.Instance;
        int characterCount =  characterControler.CharacterCount;
        
        while (CharacterIndex < characterCount && characterControler.Dead(CharacterIndex)) {
            
            CharacterIndex++;
        }

        if (CharacterIndex < characterCount) {
            
            Refresh();
            return;
        }
        
        //TODO: Select end
    }

    public void PriviousSelect() {
        CharacterIndex--;

        while (CharacterIndex >= 0 && ControleCharacterInfo.Instance.Dead(CharacterIndex)) {
            CharacterIndex--;
        }

        if (CharacterIndex >= 0) {
            Refresh();
            return;
        }

        GameFSM.Instance.DefaultPlayerTurnState();
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

        if (!Parse(buttons[Selecting]).Show)
            return;
        
        cursor.SetIndex(Selecting);
    }
    public override void SelectOut(InteractButton target) {}
    
    #endregion

    private void Input() {
            
        if (InputManager.Instance.ClickAndHold(KeyTypes.Down)) {
            NextButton();
            if (!Parse(buttons[Selecting]).Show) 
                Selecting = 0;
            
            UpdateState();
            
            cursor.SetIndex(Selecting);
        }
            
        if (InputManager.Instance.ClickAndHold(KeyTypes.Up)) {

            PriviousButton();
            if (!Parse(buttons[Selecting]).Show) {
            
                for (int i = Selecting - 1; i > -1; i--) {
                    if (Parse(buttons[i]).Show) {
                                    
                        Selecting = i;
                        break;
                    }
            
                    if (i == 0) Selecting = 0;
                }
            }
            
            UpdateState();
            
            cursor.SetIndex(Selecting);
        }
            
        if (InputManager.Instance.Click(KeyTypes.Select)) {
            
            int index = Selecting;
                
            Parse(buttons[index]).Click();
        }
        else if (InputManager.Instance.Click(KeyTypes.Cancel)) {

            PriviousSelect();
        }
    }
    
    private static SkillButton Parse(InteractButton button) {
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