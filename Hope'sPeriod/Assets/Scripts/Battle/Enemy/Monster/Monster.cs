using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Monster: MonoBehaviour {

    [SerializeField] private List<EnemyPatternBase> patterns = new();
    [field: SerializeField]public int MaxHP { get; private set; }
    public static Monster Instance { get; private set; } = null;
    public bool IsPattern { get; private set; } = false;
    private int index = 0;

    public bool StartPattern() {

        if (IsPattern)
            return false;

        IsPattern = true;
        index = Random.Range(0, patterns.Count);
        Debug.Log($"Pattern({index}) start");
        patterns[index].StartPattern();

        return true;
    }

    private void Update() {
        if (IsPattern) {

            IsPattern = patterns[index].End;
        }
    }

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

    }
}