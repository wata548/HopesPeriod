using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterSlider: Slider {

    [SerializeField] private TMP_Text shower;
    private const float Interval = 0.027f;
    private const float CharacterInterval = 0.052f; 
    
    
    public ChangeSliderState UpdateInfo(float current, float max) {

        float ratio = current / max;
        int count = (int)((ratio - Interval) / CharacterInterval);
        string context = $"{current}/{max}";
        context = new string(context?.Take(count)?.ToArray()).AddColor(Color.black) +
                  new string(context.Skip(count)?.ToArray()).AddColor(Color.white);

        Debug.Log($"maintain: {context}");
        shower.text = context;
        return base.UpdateInfo(ratio);
    }
}