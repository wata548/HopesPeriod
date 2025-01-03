using UnityEngine;

public class CompoRoute: MoveComposite {
    public CompoRoute(GameObject owner) : base(owner) {
    }

    public override int Priority { get; protected set; } = 1;
    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 10;
    public override GameObject Owner { get; protected set; }
    
    public Vector2 Target { get; private set; }

    public void SetTarget(Vector2 target) {
        Target = target;
    }

    private Vector2 before = Vector2.zero;
    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo = Direction.None) {

        Vector2 direction = Target - Owner.transform.position.ToVec2();
        direction = direction.normalized;

        if (before != Vector2.zero && !before.Approximately(direction)) {

            Power = 0;
        }
        
        before = direction;
        return direction * Power;
    }
}