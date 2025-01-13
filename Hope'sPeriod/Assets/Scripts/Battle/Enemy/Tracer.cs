using System;
using Unity.VisualScripting;
using UnityEngine;

//It is sample model
[RequireComponent(typeof(Rigidbody2D))]
public class Tracer : BaseEnemy {

    void Disactive(GameObject target) {

        if(target.transform.CompareTag("Player"))
            gameObject.SetActive(false);
    }

    public override float Damage { get; protected set; } = 10;
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; } = new TriggerType();

    [SerializeField] private float speed = 6;
    
    private static GameObject player = null;
    private Rigidbody2D enemy = null;
    
    private Vector2 before = Vector2.zero;
    public override void Move() {

        var velo = MoveComposite.Play(enemy.linearVelocity, Vector2.zero);
        enemy.linearVelocity = velo;
    } 
    private void Awake() {

        MoveComposite = new CompositeGroupBase(gameObject)
            .AddComposite(new CompoTrace(null));
        
        MoveComposite.SetPower<CompoTrace>(speed)
            .GetType<CompoTrace>()
            .SetTarget(Player);
        
        enemy ??= GetComponent<Rigidbody2D>();
        ContactStrategy.SetProcess(Disactive);

        enemy.linearVelocity = (Player.transform.position - transform.position).normalized * speed;
    }

    void Update() {
        
        Move();
    }
}