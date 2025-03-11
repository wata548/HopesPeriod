using System;
using System.CodeDom;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainButton: InteractButtonUI {
    public override void Click() {

        ToMainButtonManager(Manager).Init();
        
        Manager.SetInteractable(false);
        AudioManager.Instance.PlayOne(FmodEvents.Instance.Click);
    }

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

    public void StartButton() {

        if (!Manager.Interactable)
            return;
        
        Manager.SetInteractable(false);
        SaveData.Load();

        Warnnning.Instance
            .StartAnimation()
            .OnComplete(() => ScenceControler.Load("PlayMap"));
    }
    
    public void Awake() {

        text = GetComponentInChildren<TMP_Text>();
        fontSize = text.fontSize;
    }
    
    public static MainButtonManager ToMainButtonManager(InteractButtonManager manager) {
        if (manager is not MainButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(MainButtonManager));

        return result;
    }  
}