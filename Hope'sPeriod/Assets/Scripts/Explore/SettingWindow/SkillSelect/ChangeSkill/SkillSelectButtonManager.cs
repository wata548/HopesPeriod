using System;
using TMPro;
using UnityEngine;

public class SkillSelectButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; }
    [SerializeField] private SkillListButtonManager list;
    [SerializeField] private TMP_Text targetName;
    [SerializeField] private GameObject window;
    
    private EachCharacterInfo info;

    public void TurnOn(EachCharacterInfo characterInfo, int target) {
        Interactable = true;
        window.SetActive(true);
        
        info = characterInfo;
        int index = 0;
        foreach (var button in buttons) {
            
            Parse(button).TurnOn(info.GetSkill(index++));
        }

        targetName.text = target switch {
            0 => "해일",
            1 => "캐빌",
            2 => "리피",
            _=> "error"
        };
    }

    public void TurnOff() {
        Interactable = false;
        window.SetActive(false);
    }
    public override void SelectIn(InteractButton target) {
        Parse(target).Active();
    }

    public override void SelectOut(InteractButton target) {
        Parse(target).Disactive();
    }

    public void ListTurnOn(int index) {
        list.TurnOn(info, index);
    }
    private SkillSelectButton Parse(InteractButton button) {
        if (button is not SkillSelectButton result)
            throw new TypeMissMatched(button.gameObject, typeof(SkillSelectButton));

        return result;
    }

    private void Start() {
        TurnOff();
    }
}