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
    private Tween animation = null;
    private static readonly Color DeadColor = new(0.3f, 0.3f, 0.3f);
    
    public void SetColor(Color color) {
        symbol.color = color;
        handle.color = color;
        backGround.color = color;
    }

    private void ShowerUpdate(float current, float maximum, bool dead) {
        if (dead) {
            shower.text = $"{current}/{maximum}".AddColor(DeadColor);
            return;
        }
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
    
    public void SetState(float current, float maximum, bool dead) {

        float ratio = current / maximum;
        ShowerUpdate(current, maximum, dead);
        handle.fillAmount = ratio;
        if (dead)
            SetColor(DeadColor);
        else SetColor(Color.white);
    }
    
    public void UpdateState(float current, float maximum, bool dead) {
        
        float ratio = current / maximum;
        ShowerUpdate(current, maximum, dead);
        if (animation is null || !animation.IsComplete())
            animation.Kill();

        float size = handle.fillAmount;
        DOTween.To(x => handle.fillAmount = x, size, ratio, Duraction)
            .OnComplete(() => {
                    if (dead)
                        SetColor(DeadColor);
                    else SetColor(Color.white);
                }
            );
    }
}