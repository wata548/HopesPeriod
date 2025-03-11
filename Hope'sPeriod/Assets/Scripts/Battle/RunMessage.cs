using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RunMessage: MonoBehaviour {

    [SerializeField] private GameObject window;
    [SerializeField] private Image outline;
    [SerializeField] private Image backGround;
    [SerializeField] private TMP_Text context;
    private const float showTime = 0.2f;
    private const float disappearTime = 0.5f;


    public enum RunCase {
        Success,
        Fail,
        Boss
    }
    
    public void Show(RunCase massage, Action callback = null) {
        context.text = massage switch {
            RunCase.Success => "성공적으로 도망쳤다.",
            RunCase.Fail => "도망치는 데 실패했다.",
            RunCase.Boss => "이 적에게는 못 도망칠 것 같다."
        };

        window.SetActive(true);
        Sequence animation = DOTween.Sequence();
        animation.AppendInterval(showTime)
            .Append(outline.DOFade(0, disappearTime))
            .Join(context.DOFade(0, disappearTime))
            .Join(backGround.DOFade(0, disappearTime + 0.5f))
            .OnComplete(() => {
                window.SetActive(false);
                outline.color = Color.white;
                backGround.color = Color.black;
                context.color = Color.white;
                callback?.Invoke();
            });
    }
}