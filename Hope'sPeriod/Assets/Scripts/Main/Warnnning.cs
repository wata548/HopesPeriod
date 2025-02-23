using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Warnnning: MonoBehaviour {

    [SerializeField] private Image warnning;
    [SerializeField] private GameObject warnningBack;
    public static Warnnning Instance { get; private set; }

    public Tween StartAnimation() {
        Sequence warnningAnimation = DOTween.Sequence();
        return warnningAnimation
            .Append(warnning.DOFade(1, 1f)
                .OnStart(() => warnningBack.SetActive(true)))
            .AppendInterval(1f)
            .Append(warnning.DOFade(0, 1f));
    }

    private void Awake() {
        Instance = this;
    }
}