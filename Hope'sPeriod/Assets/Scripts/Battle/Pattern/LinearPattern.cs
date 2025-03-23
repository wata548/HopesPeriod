using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LinearPattern: BaseEnemy {
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; }
    public override float Damage { get; protected set; } = 10;

    private const float xLimit = 8.4f;
    private const float yLimit = 6f;
    private void Disactive(GameObject target) {
        if (target.transform.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
    
    protected float speed = 13;
    private Rigidbody2D rigidbody2D = null;

    protected void Start() {
        ContactStrategy = new TriggerType(Disactive);
        
        MoveComposite = new CompositeGroupBase(gameObject)
            .AddComposite(new CompoLinear(null))
            .SetPower<CompoLinear>(speed);

        rigidbody2D ??= GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;
        MoveComposite.GetComposite<CompoLinear>()
            .SetTarget(Player.transform.position)
            .SetView();
    }

    private void FixedUpdate() {

        var pos = transform.position;
        if (pos.x is <= -xLimit or >= xLimit || pos.y is <= -yLimit or >= yLimit) {
            Destroy(gameObject);
        }
    }

    public override void Move() {

        rigidbody2D.linearVelocity = MoveComposite.Play(rigidbody2D.linearVelocity, Vector2.zero);
    }
}
