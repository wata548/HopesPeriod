using System;
using System.Numerics;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

public class ButtonInteract: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler{

    [SerializeField] private ButtonType type;
    private TMP_Text text;
    private const float InteractSize = 1.2f;
    private const float Duraction = 0.2f;
    private readonly Color interactColor = Color.gray;
    private Color defaultColor;
    private Vector3 defaultSize;
    private Tween animation = null;
    private bool isOn = false;
    
    private void Awake() {

        text = GetComponentInChildren<TMP_Text>();
        defaultSize = transform.localScale;
        defaultColor = text.color;
    }

    public void StartAnimation() {
        isOn = true;
        animation = text.transform.DOScale(defaultSize * InteractSize, Duraction);
        text.color = interactColor; 
    }

    public void EndAnimation() {
        isOn = false;
        if (animation != null && animation.IsPlaying()) {
            animation.Kill();
        }
        text.transform.DOScale(defaultSize, 0);
        text.color = defaultColor; 
    }
    
    public void OnPointerEnter(PointerEventData eventData) {

        ButtonInteractManager.Instance.SelectButton(this, type);
    }

    public void OnPointerExit(PointerEventData eventData) {

        EndAnimation();
    }

    public void OnPointerMove(PointerEventData eventData) {

        if (!isOn) {
            ButtonInteractManager.Instance.SelectButton(this, type);
        }
    }
}