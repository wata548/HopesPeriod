using TMPro;
using UnityEngine;

public class SkillListButton: InteractButtonUI {

    private TMP_Text text;
    private int code;
    public bool Interactable { get; private set; } = true;
    
    private void Awake() {
        text = GetComponentInChildren<TMP_Text>();
    }
    public override void Click() {
    }
    
    public void SetCode(int code) {

        this.code = code;
        if (code == 0) {
            Interactable = false;
            text.text = "";
            return;
        }
        
        Interactable = true;
        text.text = SkillInfo.Name(code);
    }
}