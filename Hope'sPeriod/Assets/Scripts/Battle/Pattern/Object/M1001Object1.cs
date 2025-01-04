using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class M1001Object1: BaseEnemy {

    void Collision(GameObject other) {

        if (other.TryGetComponent<M1001Object1>(out M1001Object1 pattern)) {

            Damage = activeDamage;
            
        }
    }
    
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; } = new ColliderType();
    public override float Damage { get; protected set; } = 0;
    public float activeDamage = 15;
    private Rigidbody2D rigidbody2D;
    
    public override void Move() {

        rigidbody2D.linearVelocity = MoveComposite.Play(rigidbody2D.linearVelocity, Vector2.zero);
    }

    public void Awake() {

        MoveComposite = new CompositeGroupBase(gameObject);
        MoveComposite.AddComposite(new CompoLinearPos(null));
        MoveComposite.GetType<CompoLinearPos>()[0].SetTarget(new Vector2(0, 0));

        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        rigidbody2D.gravityScale = 0;
    }
}