using System;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class M1001Object3: BaseEnemy {
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; }
    public override float Damage { get; protected set; } = 10;

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
        transform.localPosition += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
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