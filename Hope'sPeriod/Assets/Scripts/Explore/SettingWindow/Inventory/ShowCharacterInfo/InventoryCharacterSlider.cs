using System;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCharacterSlider: MonoBehaviour {

    private const float Duraction = 0.5f;

    [SerializeField] private Image symbol;
    [SerializeField] private Image backGround;
    [SerializeField] private Image handle;
    [SerializeField] private TMP_Text shower;
    [SerializeField] private float intervalPercent;
    [SerializeField] private float charPercent;
    private Vector2 handleScale;
    private Tween animation = null;
    private static readonly Color DeadColor = new(0.3f, 0.3f, 0.3f);
    
    private void Setting() {

        handleScale = handle.rectTransform.localScale;
    }
    
    private void Awake() {
        Setting();
    }

    public void SetColor(Color color) {
        handle.color = color;
        backGround.color = color;
    }

    private void ShowerUpdate(float current, float maximum) {
        float ratio = current / maximum;
        
        int black = (int)((ratio - intervalPercent) / charPercent);
        bool gray = (ratio - intervalPercent) / charPercent - black >= 0.2f;
        string text = $"{current}/{maximum}";
        string blackString = new(text.Take(black)?.ToArray());
        string grayString = "";
        if(gray)
            grayString = new(text.Skip(black)?.Take(1)?.ToArray());
        string whiteString = new(text.Skip(black + Convert.ToInt32(gray))?.ToArray());
        shower.text = blackString.AddColor(Color.black) + grayString.AddColor(new Color(0.7f,0.7f,0.7f)) + whiteString.AddColor(Color.white);
    }
    
    public void SetState(float current, float maximum) {
        
        float ratio = current / maximum;
        ShowerUpdate(current, maximum);
        handle.rectTransform.localScale= new Vector2(ratio * handleScale.x, handleScale.y);
        if (current <= 0)
            SetColor(DeadColor);
        else SetColor(Color.white);
    }
    
    public void UpdateState(float current, float maximum) {
        
        float ratio = current / maximum;
        ShowerUpdate(current, maximum);
        if (animation is null || !animation.IsComplete())
            animation.Kill();
        
        animation = handle.rectTransform.DOScaleX(ratio * handleScale.x, Duraction)
            .OnComplete(() => {
                    if (current <= 0)
                        SetColor(DeadColor);
                    else SetColor(Color.white);
                }
            );
    }
}