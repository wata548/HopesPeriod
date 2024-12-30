using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tracer : BaseEnemy {


    void Disactive(GameObject target) {

        Debug.Log("fuck");
        gameObject.SetActive(false);
    }

    public override float Damage { get; protected set; } = 10;
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; } = new TriggerType();

    public override GameObject Player{ get; protected set; }
    
    [SerializeField] private GameObject playerSet = null;
    [SerializeField] private float speed = 2;
    
    private static GameObject player = null;
    private Rigidbody2D enemy = null;
    
    private void SetPlayer(GameObject player) {

        if (player.IsUnityNull())
            return;
        
        Tracer.player = player;
    }

    private void SetPlayer() {

        if (!player.IsUnityNull())
            return;

        player = playerSet;

    }

    private Vector2 before = Vector2.zero;
    public override void Move() {

        SetPlayer();

        var velo = MoveComposite.Play(enemy.linearVelocity, Vector2.zero);
        enemy.linearVelocity = velo;
    } 
    private void Awake() {

        MoveComposite = new CompositeGroupBase(gameObject)
            .AddComposite(new CompoTrace(null));
        
        MoveComposite.SetPower<CompoTrace>(speed)
            .GetType<CompoTrace>()[0]
            .SetTarget(playerSet);
        
        enemy ??= GetComponent<Rigidbody2D>();
        ContactStrategy.SetProcess(Disactive);
        Player = playerSet;
    }

    void Update() {
        
        Move();
    }
}