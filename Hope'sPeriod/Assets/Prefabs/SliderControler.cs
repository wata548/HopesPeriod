using System.IO;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public enum SliderStateChange {
    UP,
    SAME,
    DOWM
};

public class Slider : MonoBehaviour{
   
    [SerializeField] private GameObject handle = null;

    private float? handleScale = null;
    private const float DURACTION = 0.5f;
    private Tween current = null;
    
    private float beforeRatio = 1f;
    private void CheckScale() {

        if (handleScale != null)
            return;
        
        if (handle.IsUnityNull())
            throw new ForgetSetUpInspector("slider's handle Prefab");

        handleScale ??= handle.transform.localScale.x;
    }
    
    public SliderStateChange UpdateInfo(float ratio) {
        
        //Check set up
        CheckScale();
        
        SliderStateChange changeState;
        
        //Check deltaType
        if (ratio > beforeRatio)
            changeState = SliderStateChange.UP;
        else if (ratio < beforeRatio)
            changeState = SliderStateChange.DOWM;
        else
            changeState = SliderStateChange.SAME;

        //Check yet doing Dotween
        if (current == null || !current.IsComplete()) {
            
            current.Kill();
        }
        
        current = handle.transform.DOScaleX((handleScale ?? 1f) * ratio, DURACTION);

        beforeRatio = ratio;
        return changeState;
    }
}