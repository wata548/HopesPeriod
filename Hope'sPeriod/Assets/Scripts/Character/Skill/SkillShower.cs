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
    
    private void Awake() {

        nameText = nameBox.GetComponentInChildren<TMP_Text>();
        skillText = skillBox.GetComponentInChildren<TMP_Text>();
    }

    public void Show() {

        var list = SkillButtonManager.Instance.SelectList;
        List<(int user, int code, int target)> userAndCode = new();
        for (int i = 0; i < list.Count; i++) {
            if(list[i].code == 0) continue;

            userAndCode.Add((i, list[i].code, list[i].selectTarget));
        }
        
        Sequence animation = DOTween.Sequence();

        int index = 0;
        for (int i = 0; i < userAndCode.Count; i++) {

            var showAnimation = ShowAnimation();
            showAnimation.OnStart(() => {
                
                ShowSetting(userAndCode[index++]);
            });
            animation.Append(showAnimation.DOBeforeWait(Interval));
        }

        animation.OnComplete(() => GameFSM.Instance.SkipState());
    }

    private Tween ShowAnimation() {
        Sequence showing = DOTween.Sequence();
        showing.Append(nameBox.DOBlink(AppearTime, StayTime, DisappearTime))
            .Join(nameText.DOBlink(AppearTime, StayTime, DisappearTime))
            .Join(skillBox.DOBlink(AppearTime, StayTime, DisappearTime))
            .Join(skillText.DOBlink(AppearTime, StayTime, DisappearTime));
        
        return showing;
    }

    private void ShowSetting((int user, int code, int target) value) {

        ShowSetting(value.user, value.code, value.target);
    }
    
    private void ShowSetting(int index, int code, int target) {

        
        var characterInfo = CharactersInfoBattle.Instance.NameAndColor[index];
        nameText.text = characterInfo.Name;
        var personal = characterInfo.Personal;
        personal.a = 0;
        nameBox.color = personal;
        
        skillText.text = SkillInfo.Name(code);
        StartCoroutine(Wait.WaitAndDo(UseDelay, () => SkillInfo.UseSkill(index, code, target)));
    }
}