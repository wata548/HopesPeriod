using System;
using UnityEngine;
using UnityEngine.Rendering;

public enum ButtonType{
    
    Attack = 0,
    Item = 1,
    Shield = 2
}

public class ButtonInteractManager: MonoBehaviour {

    [SerializeField] private ButtonInteract[] buttons = new ButtonInteract[3];
    public static ButtonInteractManager Instance { get; private set; } = null;
    private ButtonType selectType = ButtonType.Attack;
    private bool isActive = true;
    
    public void Select(ButtonInteract target, ButtonType type) {
        foreach (var button in buttons) {
            button.EndAnimation();
        }

        selectType = type;
        target.StartAnimation();
    }
    
    private void Update() {

        if (GameFSM.Instance.State != GameState.PlayerAttack) {
            if (isActive) {
                isActive = false;
                
                foreach (var button in buttons) {
                    button.gameObject.SetActive(false);
                }
            }
            return;
        }

        if (!isActive) {
            
            isActive = true;
            foreach (var button in buttons) {
                button.gameObject.SetActive(true);
            }
        }
        
        if(InputManager.Instance.Click(KeyTypes.Right)) {
            if (selectType == ButtonType.Shield) {
                Select(buttons[(int)ButtonType.Attack], ButtonType.Attack);
            }
            else {
                
                Select(buttons[(int)selectType + 1], (ButtonType)((int)selectType + 1));
            }
        }

        if (InputManager.Instance.Click(KeyTypes.Left)) {
            if (selectType == ButtonType.Attack) {
                Select(buttons[(int)ButtonType.Shield], ButtonType.Shield);
            }
            else {
                            
                Select(buttons[(int)selectType - 1], (ButtonType)((int)selectType - 1));
            }
        }
    }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

    }
}