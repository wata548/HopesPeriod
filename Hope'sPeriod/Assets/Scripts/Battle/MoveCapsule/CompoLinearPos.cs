using DG.Tweening;
using UnityEngine;

public class CompoLinearPos: MoveComposite {
    public CompoLinearPos(GameObject owner) : base(owner) {
    }

    public override int Priority { get; protected set; } = 1;
    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 0.2f;
    public override GameObject Owner { get; protected set; }
    
    public Vector2 Target { get; private set; }

    public void SetTarget(Vector2 target) {
        Target = target;
    }

    private Vector2 before = Vector2.zero;
    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo = Direction.None) {

        Vector2 direction = Target - Owner.transform.position.ToVec2();
        var distance = direction.magnitude;
        direction = direction.normalized;
        direction *= Time.deltaTime * 60 * Power;

        if (direction.magnitude >= distance) {

            var zCoor = Owner.transform.position.z;
            Owner.transform.position = Target.ToVec3(zCoor);
        }
        else {
            Owner.transform.position += direction.ToVec3();
        }
        
        return nextVelo;
    }
}