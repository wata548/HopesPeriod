using UnityEngine;

public abstract class EnemyPatternBase : MonoBehaviour{
        
    public abstract bool Active { get; protected set; }

    public abstract void StartPattern();
}