using System;
using DG.Tweening;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;



public class MoveHand: EnemyAnimation {

    [SerializeField] private float power = 1;
    [SerializeField] private float duraction = 1; 
    [SerializeField] private Vector3 appearPos;
    [SerializeField] private Vector3 disappearPos;
    [SerializeField] private Vector3 defaultPos;
    private float seed;
    
    private void Awake() {
        seed = Random.Range(0, 10f) * 10;
    }

    public override bool UseStartEvent { get; } = false;
    public override Tween OnAnimaton() {
        var fix = (transform as RectTransform);

        return fix.DOLocalMove(defaultPos, 1.5f)
            .OnStart(() => fix.localPosition = appearPos);
    }

    public override Tween StayAnimaton() {
        transform.localPosition = 
            defaultPos + Vector3.up * Mathf.Sin((seed + Time.time) * duraction) * power;

        return null;
    }

    public override Tween OutAnimaton() {
        var fix = (transform as RectTransform);
        
        return fix.DOLocalMove(disappearPos, 1.5f);
    }
}