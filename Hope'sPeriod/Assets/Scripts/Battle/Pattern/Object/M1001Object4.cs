using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class M1001Object4: BaseEnemy {
    public override CompositeGroupBase MoveComposite { get; protected set; } = null;
    public override ContactStrategy ContactStrategy { get; protected set; } = null;
    public override float Damage { get; protected set; } = 5 * 3;

    private const float WidthLength = 8;
    private const float HeightLength = 3;
    private void Disactive(GameObject target) {
        if (target.transform.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
    
    private float speed = 10f;
    private Rigidbody2D rigidbody2D = null;

    public void SetPos(bool direction) {

        var position = new Vector3((direction ? -1 : 1) * WidthLength, Random.Range(-HeightLength, HeightLength), -1);
        transform.position = position;

        position.x *= -1;
        MoveComposite.GetType<CompoLinear>()[0]
            .SetTarget(position);

    }
    
    private void Awake() {
        this.ContactStrategy ??= new TriggerType(Disactive);

        if (MoveComposite == null) {
            
            MoveComposite = new CompositeGroupBase(gameObject)
                .AddComposite(new CompoLinear(null))
                .SetPower<CompoLinear>(speed);
        }

        rigidbody2D ??= GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {

        var pos = transform.position;
        if (pos.x is <= -10f or >= 10f) {
            Destroy(gameObject);
        }
    }

    public override void Move() {

        var direction = MoveComposite.Play(rigidbody2D.linearVelocity, Vector2.zero);
        rigidbody2D.linearVelocity = direction;
    }
}