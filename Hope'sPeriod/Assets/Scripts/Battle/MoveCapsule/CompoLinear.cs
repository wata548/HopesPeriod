using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class CompoLinear: MoveComposite {
    public CompoLinear(GameObject owner) : base(owner) {
    }

    public override int Priority { get; protected set; } = 1;
    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 10;
    public override GameObject Owner { get; protected set; }
    
    public Vector2 Target { get; private set; }
    private Vector2 direction = Vector2.one;
    
    public CompoLinear SetTarget(Vector2 target) {
        Target = target;
        direction = Target - Owner.transform.position.ToVec2();

        direction = direction.normalized;

        return this;
    }

    public CompoLinear SetView() {
        Owner.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);
        return this;
    }

    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo = Direction.None) {

        return direction * Power;
    }
}