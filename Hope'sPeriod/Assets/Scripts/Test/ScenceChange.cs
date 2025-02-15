using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScenceChangeEffecter: MonoBehaviour {
    private Image changer;
    public static ScenceChangeEffecter Instance { get; private set; } = null;
    public Tween StartEffect() {
        
        changer.fillAmount = 0;
        changer.enabled = true;
        changer.color = Color.black;
        return DOTween.To(() => changer.fillAmount, x => changer.fillAmount = x, 1f, 1.5f);
    }

    public Tween EndEffect(float duraction) {

        return changer.DOColor(new Color(0,0,0,0), duraction)
            .OnComplete(() => {
                changer.enabled = false;
                changer.fillAmount = 0;
            });
    }
    
    private void Awake() {
        changer = GetComponent<Image>();
        changer.enabled = false;

        if (Instance == null) {

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);
    }
}