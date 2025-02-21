using System;
using TMPro;
using UnityEngine;

public class SkillListButton: InteractButtonUI {

    private TMP_Text text;
    public bool On { get; private set; } = false;
    
    public override void Click() {

        if (!On)
            return;
        
        Parse(Manager).Click(Code);
    }

    public int Code { get; private set; }

    public void SetCode(int code) {
        this.Code = code;
        if (code == 0) {
            On = false;
            text.text = "";
            return;
        }

        On = true;
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