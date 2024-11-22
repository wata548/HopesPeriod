using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysics : MonoBehaviour
{
    Rigidbody2D playerRigidbody = null;

    Direction moveableDirection = DirectionInfo.ALL;
    Direction frictionDirection = DirectionInfo.ALL;
    Direction gravityDirection  = DirectionInfo.NONE;


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

    void Update()
    {
        //* Input vector
        Vector2 input = power * PlayerMovement.CalculateDirection(moveableDirection);

        bool isMoving = playerVelocity != Vector2.zero;

        if (isMoving && input == Vector2.zero) {

            //* Friction process
            float ratio = frictionPower * frictionRatio * Time.deltaTime;
            playerVelocity = PlayerMovement.CalculateFrictionPercent(frictionDirection, playerVelocity, ratio);
        }

        else if(input != Vector2.zero) {

            playerVelocity = input;
        }

        var gravity = PlayerGravity.CalculateGravity(gravityDirection);

        playerRigidbody.linearVelocity = playerVelocity + gravity;
    }
}