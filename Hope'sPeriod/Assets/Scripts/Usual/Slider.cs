using System;
using System.IO;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Slider : MonoBehaviour{
   
    [SerializeField] private GameObject handle = null;

    private float handleScale = 1;
    private const float DURACTION = 0.5f;
    private Tween current = null;
    
    private float beforeRatio = 1f;
    private void CheckScale() {

        if (handle.IsUnityNull())
            throw new ForgetSetUpInspector("slider's handle Prefab");

        handleScale = handle.transform.localScale.x;
    }

    public void Awake() {
        //Check set up
        CheckScale();
    }

    public ChangeSliderState UpdateInfo(float ratio) {
        
        ChangeSliderState changeState;
        
        //Check deltaType
        if (ratio > beforeRatio)
            changeState = ChangeSliderState.UP;
        else if (ratio < beforeRatio)
            changeState = ChangeSliderState.DOWN;
        else
            changeState = ChangeSliderState.SAME;

        //Check yet doing Dotween
        if (current == null || !current.IsComplete()) {
            
            current.Kill();
        }
        
        current = handle.transform.DOScaleX(handleScale * ratio, DURACTION);

        beforeRatio = ratio;
        return changeState;
    }
}