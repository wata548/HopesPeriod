using System;
using TMPro;
using UnityEngine;

public class ShowEffectInfo: MonoBehaviour {

    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject box;
    private RectTransform rect;
    
    public static ShowEffectInfo Instance { get; private set; } = null;

    public void TurnOff() {
        box.SetActive(false);
    }
    
    private void Awake() {
        Instance = this;
        rect = box.GetComponent<RectTransform>();
    }

    public void TurnOn() {
        box.SetActive(true);
    }
    
    public void SetData(Vector2 pos, string info) {

        pos -= new Vector2(960, 540);
        
        text.text = info;
        rect.localPosition = pos;
        
        if (pos.x > 550) rect.pivot= Vector2.one;
        else rect.pivot = Vector2.up;
    }
}