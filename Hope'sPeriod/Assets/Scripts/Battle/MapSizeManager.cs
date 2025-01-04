using System;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class MapSizeManager : MonoBehaviour {

    public static MapSizeManager Instance { get; private set; } = null;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject outline;
    private Vector2 defaultMapSize;
    private const float OutlineInterval = 0.2f;
    private const float Duraction = 0.5f;
    
    private void Awake() {

        defaultMapSize = map.transform.localScale;
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public void Resize(Vector2 size) {

        map.transform.DOScale(size, Duraction);
        outline.transform.DOScale(size.Add(OutlineInterval), Duraction);
    }

    public void Resize() {
        
        map.transform.DOScale(defaultMapSize, Duraction);
        outline.transform.DOScale(defaultMapSize.Add(OutlineInterval), Duraction);
    }
}