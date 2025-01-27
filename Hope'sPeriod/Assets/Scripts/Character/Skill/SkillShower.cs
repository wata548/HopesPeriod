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

    private const float AppearTime = 0.5f;
    private const float StayTime = 0.6f;
    private const float DisappearTime = 0.5f;
    
    private TMP_Text nameText;
    private TMP_Text skillText;
    
    private void Awake() {

        nameText = nameBox.GetComponentInChildren<TMP_Text>();
        skillText = skillBox.GetComponentInChildren<TMP_Text>();
    }

    public void Show() {

        var list = SkillButtonManager.Instance.SelectList;
        List<(int, int)> userAndCode = new();
        for (int i = 0; i < list.Count; i++) {
            if(list[i] == 0) continue;

            userAndCode.Add((i, list[i]));
        }
        
        Sequence animation = DOTween.Sequence();

        int index = 0;
        for (int i = 0; i < userAndCode.Count; i++) {

            var showAnimation = ShowAnimation();
            showAnimation.OnStart(() => ShowSetting(userAndCode[index++]));
            animation.Append(showAnimation.DOBeforeWait(0.1f));
        }
    }

    private Tween ShowAnimation() {
        Sequence showing = DOTween.Sequence();
        showing.Append(nameBox.DOBlink(AppearTime, StayTime, DisappearTime))
            .Join(nameText.DOBlink(AppearTime, StayTime, DisappearTime))
            .Join(skillBox.DOBlink(AppearTime, StayTime, DisappearTime))
            .Join(skillText.DOBlink(AppearTime, StayTime, DisappearTime));
        
        return showing;
    }

    private void ShowSetting((int user, int code) value) {

        Debug.Log($"{value.user}: {value.code}");
        ShowSetting(value.user, value.code);
    }
    
    private void ShowSetting(int index, int code) {

        Debug.Log($"{index}: {code}");
        
        var characterInfo = ControleCharacterInfo.Instance.NameAndColor[index];
        nameText.text = characterInfo.Name;
        var personal = characterInfo.Personal;
        personal.a = 0;
        nameBox.color = personal;
        
        skillText.text = SkillInfo.Name(code);
    }
}