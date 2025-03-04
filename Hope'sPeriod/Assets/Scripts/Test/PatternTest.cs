using System;
using Unity.VisualScripting;
using UnityEngine;

public class PatternTest: MonoBehaviour {
    [SerializeField] private EnemyPatternBase pattern;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {

            if (pattern.Active) return;
            pattern.StartPattern();
        }
    }
}