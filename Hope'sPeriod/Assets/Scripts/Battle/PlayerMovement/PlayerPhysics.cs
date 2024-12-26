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

    private void Awake() {

        SettingCollider.SetCollider(gameObject);

        if (playerRigidbody == null) {

            playerRigidbody = GetComponent<Rigidbody2D>();
        }

    }

    private MoveComposite a = new CompoDefault();
    void Update() {

        playerRigidbody.linearVelocity = a.Play(playerRigidbody.linearVelocity, Vector2.zero);
        /*
        //* Input vector
        Vector2 input = power * PlayerMovement.CalculateDirection(moveableDirection);

        bool isMoving = !UsualVector.Approximately(playerVelocity, Vector2.zero);
        bool isInput = !UsualVector.Approximately(input, Vector2.zero);

        if (isMoving && !isInput) {

            //* Friction process
            float ratio = frictionPower * frictionRatio * Time.deltaTime;
            playerVelocity = PlayerMovement.CalculateFrictionPercent(frictionDirection, playerVelocity, ratio);
        }

        else if(input != Vector2.zero) {

            playerVelocity = input;
        }

        var gravity = PlayerGravity.CalculateGravity(gravityDirection);

        playerRigidbody.linearVelocity = playerVelocity + gravity;
        */
    }
}
