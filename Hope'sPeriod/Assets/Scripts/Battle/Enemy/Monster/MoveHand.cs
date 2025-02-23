using System;
using DG.Tweening;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum HandState {
    On,
    Off,
    Default
}

public class MoveHand: MonoBehaviour {

    [SerializeField] private float power = 1;
    [SerializeField] private float duraction = 1; 
    [SerializeField] private Vector3 appearPos;
    [SerializeField] private Vector3 disappearPos;
    [SerializeField] private Vector3 defaultPos;
    private float seed;
    public HandState state = HandState.Default; 
    private bool isActive = false;
    
    private void Awake() {
        seed = Random.Range(0, 1f);
    }

    private void Update() {


        if (!isActive) {

            if (GameFSM.Instance.State == GameState.PlayerAttack) {
                isActive = true;
                state = HandState.On;
            }
            else return;
        }
        else if(GameFSM.Instance.State != GameState.PlayerAttack) {
            state = HandState.Off;
        }    
            
        if (state == HandState.Default) {
            
            transform.localPosition = 
                defaultPos + Vector3.up * Mathf.Sin(seed + Time.time * duraction) * power;
        }
            
        else if (state == HandState.Off) {
            var fix = (transform as RectTransform);
            fix.DOLocalMove(disappearPos, 1.5f)
                .OnComplete(() => {
                    isActive = false;
                    state = HandState.Default;
                });
        }
        else {
            var fix = (transform as RectTransform);
            fix.DOLocalMove(defaultPos, 1.5f)
                .OnStart(() => fix.localPosition = appearPos)
                .OnComplete(() => {
                    state = HandState.Default;
                });
        }   
    }
}