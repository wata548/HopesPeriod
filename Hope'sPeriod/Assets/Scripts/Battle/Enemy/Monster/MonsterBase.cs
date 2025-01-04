using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MonsterBase: MonoBehaviour {

    [SerializeField] private List<EnemyPatternBase> patterns = new();
    public static MonsterBase Instance { get; private set; } = null;
    
    [SerializeField] int maxHP;
    public int MaxHP {
        get => maxHP;
        private set => maxHP = value;
    }

    public int CurrentHP { get; private set; }
    
    private int index = 0;
    public bool IsPattern { get; private set; } = false;
    
    private bool StartPattern() {

        if (IsPattern)
            return false;

        IsPattern = true;
        index = Random.Range(0, patterns.Count);
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