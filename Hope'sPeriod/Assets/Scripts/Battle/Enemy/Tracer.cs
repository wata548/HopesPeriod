using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tracer : BaseEnemy {


    void Disactive(GameObject target) {

        target.SetActive(false);
    }
    
    public override float Damage { get; protected set; } = 10;
    public override ContactStrategy Strategy { get; protected set; } = new TriggerType();
    public override GameObject ProcessTarget { get; protected set; }
    
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

        if (playerSet.IsUnityNull())
            throw new ForgetSetUpInspector("player object");

        player = playerSet;

    }

    private Vector2 before = Vector2.zero;
    public override void Move() {

        SetPlayer();

        var pos = transform.localPosition.ToVec2();
        var destination = player.transform.localPosition.ToVec2();
        var velo = LinearMoveVelo(pos, destination, speed);

        velo = Vector2.Lerp(before, velo, Time.deltaTime);
        before = velo;
        enemy.linearVelocity = velo;
    }

    private void Awake() {

        enemy ??= GetComponent<Rigidbody2D>();
        Strategy.SetProcess(Disactive);
        ProcessTarget = gameObject;
    }

    void Update() {
        
        Move();
    }
}