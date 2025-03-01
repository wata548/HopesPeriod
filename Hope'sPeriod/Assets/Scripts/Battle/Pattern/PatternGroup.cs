using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternGroup: EnemyPatternBase {
    public override bool Active { get; protected set; } = false;
    [SerializeField] private List<EnemyPatternBase> patterns;
    
    private IEnumerator pattern() {

        Active = true;
        foreach (var pattern in patterns)
            pattern.StartPattern();
        bool result = false;
        while (!result) {

            yield return null;
            
            bool temp = true;
            foreach (var pattern in patterns) {
                temp &= !pattern.Active;
            }

            result = temp;
        }

        Active = false;
    }
    public override void StartPattern() {
        StartCoroutine(pattern());
    }
}