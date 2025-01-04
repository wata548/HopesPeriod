using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TilePlayerPhysics : MonoBehaviour
{
    Rigidbody2D playerRigidbody = null;
    private PlayerAnimation animation = null;
    
    Direction moveableDirection = DirectionInfo.All;
    Direction frictionDirection = DirectionInfo.All;

    float power = 5f;
    float frictionRatio = 0.5f;
    float frictionPower = 20;

    Vector2 playerVelocity = Vector2.zero;

    private CompositeGroupBase movement;
    private void Awake() {

        animation ??= GetComponent<PlayerAnimation>();
        movement = new CompositeGroupBase(gameObject)
            .AddComposite(new CompoInput(null))
            .AddComposite(new CompoFriction(null))
            .SetApply<CompoInput>(moveableDirection)
            .SetPower<CompoInput>(power)
            .SetApply<CompoFriction>(frictionDirection)
            .SetPower<CompoFriction>(frictionPower * frictionRatio);
        movement.SetCollider(new SettingCollider(gameObject));
        
        if (playerRigidbody == null) {

            playerRigidbody = GetComponent<Rigidbody2D>();
        }

    }

    void Update() {

        var velocity = movement.Play(playerRigidbody.linearVelocity, Vector2.zero);
        playerRigidbody.linearVelocity = velocity;
        animation.SetAnimation(velocity);
    }
}