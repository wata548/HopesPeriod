using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class StatePatterns {
    [field: SerializeField] 
    public float State { get; private set; }
    [field: SerializeField]
    public List<EnemyPatternBase> Patterns { get; private set; }
}

public class Monster: MonoBehaviour {

    [SerializeField] private List<StatePatterns> patterns = new();
    [field: SerializeField]public bool IsBoss { get; private set; }
    [field: SerializeField]public int MaxHP { get; private set; }
    public static Monster Instance { get; private set; } = null;
    public bool IsShowingPattern { get; private set; } = false;
    private EnemyPatternBase currentPattern = null;

    public bool StartPattern() {

        if (IsShowingPattern)
            return false;

        IsShowingPattern = true;
        
        float hpPercent = (float)MonsterSlider.Instance.CurrentHp / MaxHP;
        int index = 0;
        while(index < patterns.Count && hpPercent > patterns[index].State) {
            index++;
        }

        int randomIndex = Random.Range(0, patterns[index].Patterns.Count);
        currentPattern = patterns[index].Patterns[randomIndex];
        currentPattern.StartPattern();

        return true;
    }

    private void Update() {
        if (IsShowingPattern) {
            IsShowingPattern = currentPattern.Active;
        }
    }

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}