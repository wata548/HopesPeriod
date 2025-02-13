using System;
using TMPro;
using UnityEngine;

public class SkillListButton: InteractButtonUI {

    private TMP_Text text;
    private bool on = false;
    public override void Click() {
    }

    public int Code { get; private set; }

    public void SetCode(int code) {
        if (code == 0) {
            on = false;
            text.text = "";
            return;
        }

        on = true;
        text.text = SkillInfo.Name(code);
    }
    
    public void SetColor(Color color) {
        text.color = color;
    }
    
    public void SetUp() {
        text = GetComponentInChildren<TMP_Text>();
    }

    private SkillListButtonManager Parse(InteractButtonManager manager) {
        if (manager is not SkillListButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(SkillListButtonManager));
        return result;
    }
}