using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysics : MonoBehaviour
{
    Rigidbody2D playerRigidbody = null;

    Direction moveableDirection = DirectionInfo.All;
    Direction frictionDirection = DirectionInfo.All;
    Direction gravityDirection  = Direction.None;

    float power = 5f;
    float frictionRatio = 0.5f;
    float frictionPower = 20;

    Vector2 playerVelocity = Vector2.zero;

    public CompositeGroupBase Movement;
    private void Awake() {

        Movement = new CompoDefaultGroup(gameObject);
        Movement.SetCollider(new SettingCollider(gameObject))
            .SetApply<CompoInput>(moveableDirection)
            .SetPower<CompoInput>(power)
            .SetApply<CompoFriction>(frictionDirection)
            .SetPower<CompoFriction>(frictionPower * frictionRatio)
            .SetApply<CompoGravity>(gravityDirection);
        
        if (playerRigidbody == null) {

            playerRigidbody = GetComponent<Rigidbody2D>();
        }

    }

    void Update() {
        if (CharactersInfoBattle.Instance.GameOver) {

            playerRigidbody.linearVelocity = Vector2.zero;
            return;
        }
        playerRigidbody.linearVelocity = Movement.Play(playerRigidbody.linearVelocity, Vector2.zero);
    }
}