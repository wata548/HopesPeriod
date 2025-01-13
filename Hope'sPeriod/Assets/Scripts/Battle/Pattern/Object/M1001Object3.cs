using System;
using System.Security.Cryptography;
using UnityEngine;

public class M1001Object3: BaseEnemy {
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; }
    public override float Damage { get; protected set; } = 10 * 3f;

    private void Disactive(GameObject target) {
        if (target.transform.CompareTag("Player")) {
        }
    }
    
    private float speed = 10;
    private Rigidbody2D rigidbody2D = null;
    
    private void Start() {
        this.ContactStrategy = new TriggerType(Disactive);
        
        MoveComposite = new CompositeGroupBase(gameObject)
            .AddComposite(new CompoLinear(null))
            .SetPower<CompoLinear>(speed);

        rigidbody2D ??= GetComponent<Rigidbody2D>();
        var position = transform.position;
        position.y = -10;
        MoveComposite.GetType<CompoLinear>().SetTarget(position);
    }

    private void FixedUpdate() {

        var pos = transform.position;
        if (pos.y is <= -5.5f or >= 5.5f) {
            Destroy(gameObject);
        }
    }

    public override void Move() {

        rigidbody2D.linearVelocity = MoveComposite.Play(rigidbody2D.linearVelocity, Vector2.zero);
    }
}