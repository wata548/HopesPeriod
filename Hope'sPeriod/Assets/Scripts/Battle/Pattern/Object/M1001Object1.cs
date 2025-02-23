using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class M1001Object1: BaseEnemy {

    public override CompositeGroupBase MoveComposite { get; protected set; } = null;
    public override ContactStrategy ContactStrategy { get; protected set; } = new TriggerType();
    public override float Damage { get; protected set; } = 30;
    private Rigidbody2D rigidbody2D;
    
    public override void Move() {}

    public void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        rigidbody2D.gravityScale = 0;
    }
}