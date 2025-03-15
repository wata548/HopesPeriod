using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MessageWindow: MonoBehaviour {

    public static MessageWindow Instance { get; private set; } = null;
    private static readonly Color Transparent = new Color(1,1,1, 0);
    
    [SerializeField] private Image window;
    [SerializeField] private TMP_Text context;
    [SerializeField] private GameObject closeButton;

    private bool on = false;
    private const float StartAppearPoint = -130f;
    private const float CompleteAppearPoint = 0f;
    private const float AppearTime = 0.55f;
    private const float MoveTime = 0.6f;

    public bool On => on;
    private Tween showAnimation = null;
    
    public void TurnOff() {

        if (showAnimation is not null)
            showAnimation.Kill();
        
        on = false;
        
        window.color = Transparent;
        window.transform.localPosition = new(0, StartAppearPoint);
        window.gameObject.SetActive(false);
        
        //check on script event
        if (ScriptShower.Instance.EventCode == 0) {
            
            TilePlayerPhysics.SetMovable(true);
            SettingWindow.SetInteractable(true);
        }

        CheckEvent.AutoEvent(TilePlayerPhysics.Pos);
        EverytimeEvent.CheckEvent();
    }

    public void TurnOn(string info) {

        on = true;
        Sequence appear = DOTween.Sequence(); 
        context.text = info;
        
        window.gameObject.SetActive(true);

        appear.Join(window.DOColor(Color.white, AppearTime))
            .Join(window.transform.DOLocalMoveY(CompleteAppearPoint, MoveTime)
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    closeButton.gameObject.SetActive(true);
                }))
            .Join(context.DOFade(1, AppearTime));
        TilePlayerPhysics.SetMovable(false);
        SettingWindow.SetInteractable(false);

        showAnimation = appear;
    }

    private void Awake() {
        Instance = this;
    }
    
    private void Update() {

        if (!on)
            return;

        if (InputManager.Instance.Click(KeyTypes.Interaction)) {

            TurnOff();
        } 
    }
    
}
