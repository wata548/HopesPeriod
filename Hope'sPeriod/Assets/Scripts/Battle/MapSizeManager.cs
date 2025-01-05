using System;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MapSizeManager : MonoBehaviour {

    public static MapSizeManager Instance { get; private set; } = null;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject outline;
    private Vector2 defaultMapSize;
    private const float OutlineInterval = 0.2f;
    private const float Duration = 0.5f;
    
    private void Awake() {

        defaultMapSize = map.transform.localScale;
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public void Move(Vector3 pos) {

        map.transform.DOLocalMove(pos, Duration).SetEase(Ease.InOutCubic);
        outline.transform.DOLocalMove(pos, Duration).SetEase(Ease.InOutCubic);
    }

    public void Move() {
        Move(Vector3.zero);
    }
    
    public void Resize(Vector2 size) {

        map.transform.DOScale(size, Duration).SetEase(Ease.InOutCubic);
        outline.transform.DOScale(size.Add(OutlineInterval), Duration).SetEase(Ease.InOutCubic);
    }

    public void Resize() {
        
        map.transform.DOScale(defaultMapSize, Duration).SetEase(Ease.InOutCubic);
        outline.transform.DOScale(defaultMapSize.Add(OutlineInterval), Duration).SetEase(Ease.InOutCubic);
    }

    public void Default() {
        Resize();
        Move();
    }
}