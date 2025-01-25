using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlider: Slider {

    [SerializeField] private SpriteRenderer outLine;
    [SerializeField] private TMP_Text shower;
    private const float Interval = 0.027f;
    private const float CharacterInterval = 0.052f;

    
    public void ChangeColor(Color color) {
        outLine.color = color;
        handleRenderer.color = color;
    }

    private void Setting(float current, float max) {
        float ratio = current / max;
        int count = (int)((ratio - Interval) / CharacterInterval);
                
        string context = $"{current}/{max}";
        context = new string(context.Take(count)?.ToArray()).AddColor(Color.black) +
                  new string(context.Skip(count)?.ToArray()).AddColor(Color.white);
        
        shower.text = context;
    }
    
    public ChangeSliderState UpdateInfo(float current, float max) {

        Setting(current, max);    
        return base.UpdateInfo(current / max);
    }

    public void SetInfo(float current, float max) {
        
        Setting(current, max);
        base.SetInfo(current / max);
    }

    private void Awake() {
        base.Awake();
    }
}