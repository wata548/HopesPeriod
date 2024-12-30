using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public sealed class CompoTrace : MoveComposite {
    public override int Priority { get; protected set; } = 5;

    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 2;
    private float ratio = 0.5f;

    private GameObject target = null;
    public override GameObject Owner { get; protected set; }

    public CompoTrace(GameObject owner) : base(owner) {}
    
    public CompoTrace SetTarget(GameObject target) {
        this.target = target;
        return this;
    }
    
    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo) {
        
        if(target.IsUnityNull()) 
            return Vector2.zero;

        var direction = target.transform.position - Owner.transform.position;
        direction = direction.normalized * Power;
        currentVelo = currentVelo.normalized;

        nextVelo = Vector2.Lerp(currentVelo, direction, Time.deltaTime * 60 * ratio);
        return nextVelo;
    }
}