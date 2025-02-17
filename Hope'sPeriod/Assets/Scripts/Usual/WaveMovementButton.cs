using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WaveMovementButton: MonoBehaviour {

    [SerializeField] private float power = 1;
    [SerializeField] private float delay = 1;
    private bool on = false;
    private Image image;
    private float start = 0;
    private Vector2 pos; 
    
    private void Awake() {
        image = GetComponent<Image>();
        image.color = new Color(0,0,0,0);
        pos = transform.localPosition;
    }

    private void Update() {

        if (!on) return;
        
        transform.localPosition = pos + new Vector2(0, Mathf.Cos((Time.time - start) / delay) * power - power);
    }

    public void TurnOn() {
        image.enabled = true;
        start = Time.time;
        on = true;
    }

    public void TurnOff() {
        image.enabled = false;
        transform.localPosition = pos;
        on = false;
    }
}