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
    private const float ActiveSize = 1.1f;
    private const float ActiveDuraction = 0.2f;
    private static int select = -1;
    
    private void SetCode(int code) {
        this.code = code;
        on = code != 0;
    }

    public void TurnOn(int code) {
        SetCode(code);

        button.SetActive(true);
        if (!on) {
            
            skillName.color = Color.gray;
            skillName.text = "빈 슬롯";
            return;
        }

        skillName.color = Color.white;
        skillName.text = SkillInfo.Name(this.code);
    }

    public void TurnOff() {
        button.SetActive(false);
    }

    private void Active() {
        if (animation != null) animation.Kill();
        animation = button.transform.DOScale(ActiveSize, ActiveDuraction);
    }

    private void Disactive() {
        if (animation != null) animation.Kill();
        animation = button.transform.DOScale(1, 0);
    }
    
    public override void Click() {
        if (select != -1) {
            Parse(Manager).GetButton(select).Disactive();
        }

        select = Index;
        Active();
        
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