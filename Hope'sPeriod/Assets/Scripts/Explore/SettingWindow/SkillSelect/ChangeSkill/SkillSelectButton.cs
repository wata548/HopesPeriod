using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SkillSelectButton: InteractButtonUI {

    [SerializeField] private GameObject button;
    private int code;
    private bool on = false;
    private TMP_Text skillName;

    private Tween animation = null;
    private const float ActiveSize = 1.2f;
    private const float ActiveDuraction = 0.2f;
    
    private void SetCode(int code) {
        this.code = code;
        on = code != 0;
    }

    public void TurnOn(int code) {
        SetCode(code);
        
        button.SetActive(on);
        if (!on) {
            skillName.text = "";
            return;
        }

        skillName.text = SkillInfo.Name(this.code);
    }

    public void Active() {
        if (animation != null) animation.Kill();
        animation = button.transform.DOScale(ActiveSize, ActiveDuraction);
    }

    public void Disactive() {
        if (animation != null) animation.Kill();
        animation = button.transform.DOScale(1, 0);
    }
    
    public override void Click() {
        Parse(Manager).ListTurnOn(Index);
    }

    private void Awake() {
        skillName = button.GetComponentInChildren<TMP_Text>();
    }

    private SkillSelectButtonManager Parse(InteractButtonManager manager) {
        if (manager is not SkillSelectButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(SkillSelectButtonManager));

        return result;
    }
}