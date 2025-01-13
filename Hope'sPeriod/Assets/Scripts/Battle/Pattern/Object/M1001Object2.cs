using System;
using System.Security.Cryptography;
using UnityEngine;

public class M1001Object2: BaseEnemy {
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; }
    public override float Damage { get; protected set; } = 10 * 3f;

    private void Disactive(GameObject target) {
        if (target.transform.CompareTag("Player")) {
            Destroy(gameObject);
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
        MoveComposite.GetType<CompoLinear>()[0].SetTarget(Player.transform.position);
    }

    private void FixedUpdate() {

        var pos = transform.position;
        if (pos.x is <= -8.4f or >= 8.4f || pos.y is <= -4.5f or >= 4.5f) {
            Destroy(gameObject);
        }
    }

    public override void Move() {

        rigidbody2D.linearVelocity = MoveComposite.Play(rigidbody2D.linearVelocity, Vector2.zero);
    }
}