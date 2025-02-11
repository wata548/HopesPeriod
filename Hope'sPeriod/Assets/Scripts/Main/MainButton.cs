using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MainButton: InteractButtonUI {
    public override void Click() {}

    private float fontSize;
    private const float ActivePower = 1.2f;
    private const float Duraction = 0.15f;
    private static readonly Color activeColor = Color.white;
    private static readonly Color disactiveColor = Color.gray;
    private TMP_Text text;
    private Tween animation = null;
    public void Active() {

        if (animation != null) {

            animation.Kill();
        }

        text.color = activeColor;
        animation = DOTween.To(x => text.fontSize = x, fontSize, fontSize * ActivePower, Duraction);
    }

    public void Disactive() {
        if (animation != null) {
        
            animation.Kill();
        }

        text.color = disactiveColor;
        text.fontSize = fontSize;
    }

    public void Awake() {

        text = GetComponentInChildren<TMP_Text>();
        fontSize = text.fontSize;
    }
}