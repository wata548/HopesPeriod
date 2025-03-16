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
    private Vector2 defaultMapPos;
    private const float OutlineInterval = 0.2f;
    private const float Duration = 0.5f;
    
    private void Awake() {

        defaultMapSize = map.transform.localScale;
        defaultMapPos = map.transform.localPosition;
        Instance = this;
    }

    public void Move(Vector3 pos, float duration = Duration) {

        map.transform.DOLocalMove(pos, duration).SetEase(Ease.InOutCubic);
        pos.z += 0.1f;
        outline.transform.DOLocalMove(pos, duration).SetEase(Ease.InOutCubic);
    }

    public void Move(float duration = Duration) {
        Move(Vector3.zero, duration);
    }
    
    public void Resize(Vector2 size, float duration = Duration) {

        map.transform.DOScale(size, duration).SetEase(Ease.InOutCubic);
        outline.transform.DOScale(size.Add(OutlineInterval), duration).SetEase(Ease.InOutCubic);
    }

    public void Resize(float duration = Duration) {
        Resize(defaultMapSize, duration);
    }

    public void Default(float duration = Duration) {
        Resize(defaultMapSize, duration);
        Move(defaultMapPos, duration);
    }
}