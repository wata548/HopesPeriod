using UnityEngine;

public class CompoLinear: MoveComposite {
    public CompoLinear(GameObject owner) : base(owner) {
    }

    public override int Priority { get; protected set; } = 1;
    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 1;
    public override GameObject Owner { get; protected set; }
    
    public Vector2 Target { get; private set; }

    public void SetTarget(Vector2 target) {
        Target = target;
    }
    
    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo = Direction.None) {

        Vector2 direction = Target - Owner.transform.position.ToVec2();
        direction = direction.normalized;
        return direction * Power;
    }
}