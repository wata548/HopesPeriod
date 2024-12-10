using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tracer : BaseEnemy {

    public override float Damage { get; protected set; } = 10;

    [SerializeField] private GameObject playerSet = null;
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

    public override void Move() {

        SetPlayer();

        var pos = transform.localPosition.ToVec2();
        var destination = player.transform.localPosition.ToVec2();
        var velo = LinearMoveVelo(pos, destination, 1f);

        enemy.linearVelocity = velo;
    }

    private void Awake() {

        if (enemy == null) {
            enemy = GetComponent<Rigidbody2D>();
        }
    }

    void Update() {
        
        Move();
    }
}
