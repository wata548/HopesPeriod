using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class SkillShower: MonoBehaviour {

    [SerializeField] private Image nameBox;
    [SerializeField] private Image skillBox;

    private const float AppearTime = 0.3f;
    private const float StayTime = 1.8f;
    private const float DisappearTime = 0.3f;
    private const float Interval = 0.1f;
    private const float UseDelay = 0.35f;
    
    private TMP_Text nameText;
    private TMP_Text skillText;
    
    private List<(int user, int code, int duraction)> remainDamage;
    private void Awake() {

        nameText = nameBox.GetComponentInChildren<TMP_Text>();
        skillText = skillBox.GetComponentInChildren<TMP_Text>();
        remainDamage = new();
    }

    public Tween Show() {

        var list = SkillButtonManager.Instance.SelectList;
        List<(int user, int code, int target)> userAndCode = new();
        for (int i = 0; i < list.Count; i++) {
            if(list[i].code == 0) continue;

            userAndCode.Add((i, list[i].code, list[i].selectTarget));
        }
        
        Sequence animation = DOTween.Sequence();

        //Duraction Demage skill
        int index = 0;
        for (int i = 0, size = remainDamage.Count; i < size; i++) {
            var showAnimation = ShowAnimation();
            showAnimation.OnStart(() => {
                ShowSetting(remainDamage[index++], true);
            });
            animation.Append(showAnimation.DOBeforeWait(Interval));
            remainDamage[i] = (remainDamage[i].user, remainDamage[i].code, remainDamage[i].duraction - 1);
        }

        //SimpleSkill
        int index2 = 0;
        for (int i = 0; i < userAndCode.Count; i++) {

            var showAnimation = ShowAnimation();
            showAnimation.OnStart(() => {
                
                ShowSetting(userAndCode[index2++]);
            });
            animation.Append(showAnimation.DOBeforeWait(Interval));

            int duraction = SkillInfo.AttackDuration(userAndCode[i].code) - 1;
            if (duraction > 0) {
                remainDamage.Add((userAndCode[i].user, userAndCode[i].code, duraction));
            }
        }

        return animation;
    }

    public void UpdateRemainInfo() {
        remainDamage = remainDamage.Where(factor => factor.duraction > 0).ToList();
    }
    
    private Tween ShowAnimation() {
        Sequence showing = DOTween.Sequence();
        showing.Append(nameBox.DOBlink(AppearTime, StayTime, DisappearTime))
            .Join(nameText.DOBlink(AppearTime, StayTime, DisappearTime))
            .Join(skillBox.DOBlink(AppearTime, StayTime, DisappearTime))
            .Join(skillText.DOBlink(AppearTime, StayTime, DisappearTime));
        
        return showing;
    }

    private void ShowSetting((int user, int code, int target) value, bool justDamage = false) {

        ShowSetting(value.user, value.code, value.target, justDamage);
    }
    
    private void ShowSetting(int index, int code, int target, bool justDamage) {

        var characterInfo = CharactersInfoBattle.Instance.NameAndColor[index];
        nameText.text = characterInfo.Name;
        var personal = characterInfo.Personal;
        personal.a = 0;
        nameBox.color = personal;
        
        skillText.text = SkillInfo.Name(code);
        Action skill;
        if (justDamage)
            skill = () => MonsterSlider.Instance.GetDamage(SkillInfo.Attack(code));
        else
            skill = () => SkillInfo.UseSkill(index, code, target);
        StartCoroutine(Wait.WaitAndDo(UseDelay, skill));
    }
}