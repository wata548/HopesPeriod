using UnityEngine;

public abstract class EnemyPatternBase : MonoBehaviour{
        
    public abstract bool End { get; protected set; }

    public abstract void StartPattern();
}