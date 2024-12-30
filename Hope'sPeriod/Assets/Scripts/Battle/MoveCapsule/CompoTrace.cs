using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public sealed class CompoTrace : MoveComposite {
    public override int Priority { get; protected set; } = 5;

    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 2;
    private float limitAngle = 2f;
    private float ditectRange = 3f;
    
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

        var v1 = (target.transform.position - Owner.transform.position).ToVec2();

        if (v1.magnitude > ditectRange) {
            return currentVelo;
        }
        v1 = v1.normalized;
        
        Vector2 v2 = (currentVelo.GetDegree() + limitAngle).GetVector();
        if (v1.InnerProduction(currentVelo) > v2.InnerProduction(currentVelo)) {
            return v1 * Power;
        }
        else {
            Vector2 v3 = (currentVelo.GetDegree() - limitAngle).GetVector();
            if (v2.InnerProduction(v1) > v3.InnerProduction(v1)) {
                return v2 * Power;
            }
            else {
                return v3 * Power;
            }
        }
    }
}