using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveMouses: EnemyAnimation {
    public override bool UseStartEvent { get; } = false;
    
    [Header("Some Info")]
    [SerializeField] private bool isRight;
    [SerializeField] private List<GameObject> mouses;
    
    [Header("Speed")]
    [SerializeField] private float power = 1;
    [SerializeField] private float duration = 1; 
    [SerializeField] private float disappearTime = 1; 
    [SerializeField] private float appearTime = 1;
    
    private List<(float seed, Vector3 pos, float power, float duraction)> mousesSeed = new();
    private const float alphaTime = 0.5f;
    private const float startX = 1085;
    private const float endX = 850;
    private const float alphaX = 100;
    private const float alphaPower = 5;
    private const float alphaDuration = 0.5f;

    public override Tween OnAnimaton() {

        Sequence animation = DOTween.Sequence();
        mousesSeed.Clear();
        foreach (var mouse in mouses) {

            float targetPos = endX + Random.Range(0, alphaX);
            targetPos *= isRight
                ? 1
                : -1;
            float targetTime = appearTime + Random.Range(0, appearTime);

            animation.Join(mouse.transform.DOLocalMoveX(targetPos, targetTime));
            var pos = mouse.transform.localPosition;
            pos.x = targetPos;
            
            mousesSeed.Add((Random.Range(0, 1f), pos, Random.Range(0, alphaPower), Random.Range(0, alphaDuration)));
        }
        return animation;
    }

    public override Tween StayAnimaton() {

        for (int i = 0; i < mouses.Count; i++) {
            
            var delta = (mousesSeed[i].seed + Time.time) * (duration + mousesSeed[i].duraction);
            mouses[i].transform.localPosition = 
                mousesSeed[i].pos + Vector3.up * Mathf.Sin(delta) * (power + mousesSeed[i].power);
        }

        return null;
    }

    public override Tween OutAnimaton() {

        Sequence animation = DOTween.Sequence();
        foreach (var mouse in mouses) {

            animation.Join(mouse.transform.DOLocalMoveX(startX * (isRight ? 1 : -1), disappearTime));
        }
        return animation;
    }
}