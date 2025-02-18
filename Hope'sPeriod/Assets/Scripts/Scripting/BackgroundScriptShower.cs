using System;
using TMPro;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BackgroundScriptShower: MonoBehaviour {
    [SerializeField] private Image box;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text context;
    [SerializeField] private WaveMovementButton skipButton;
    private const float Interval = 0.1f;
    private const float AutoSkipSecond = 3f;
    private float time;
    
    private bool start = false;
    private bool showAll  = false;
    public bool End { get; private set; } = false;

    public void Use() {
        End = false;
    }
    
    public void TurnOn() {
        box.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
    }
    
    public void TurnOff() {
        box.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }
    
    public void Erase() {
        nameText.text = "";
        context.text = "";
    }

    private int updateCount = 0;
    private void Update() {

        if (!start)
            return;
        
        updateCount++;
        if (updateCount % 2 == 0) {
            updateCount = 0;
            context.EffectProcedure();
        }

        if (showAll && Time.time - time >= AutoSkipSecond) {
            showAll = false;
            start = false;
            End = true;
        }
    }

    public void Next() {

        Debug.Log("Click");
        if (!start)
            return;
        
        if (showAll) {
            showAll = false;
            start = false;
            End = true;
            return;
        }

        skip = true;
    }

    private bool skip = false;
    public void ShowScript(int actor, string context) {

        start = true;
        showAll = false;

        Erase();
        nameText.text = ActorInfo.Name(actor);
        StartCoroutine(this.context.Typing(context, Interval, () => {
            showAll = true;
            time = Time.time;
            skip = false;
        }, () => skip));
        skipButton.TurnOn();
    }
    
}