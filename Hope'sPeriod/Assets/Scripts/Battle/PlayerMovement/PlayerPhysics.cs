using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysics : MonoBehaviour
{
    Rigidbody2D playerRigidbody = null;

    Direction moveableDirection = DirectionInfo.ALL;
    Direction frictionDirection = DirectionInfo.ALL;

    float power = 5f;
    float frictionRatio = 0.5f;
    float frictionPower = 20;

    private void Awake() {

        if (playerRigidbody == null) {

            playerRigidbody = GetComponent<Rigidbody2D>();
        }

        SettingCollider.SetCollider(gameObject);
    }

    void Update()
    {
        Vector2 velocity    = playerRigidbody.linearVelocity;
        Vector2 input       = power * PlayerMovement.CalculateDirection(moveableDirection);

        
        if (input == Vector2.zero) {

            if(velocity == Vector2.zero) {
                return;
            }

            float ratio = frictionPower * frictionRatio * Time.deltaTime;
            velocity = PlayerMovement.CalculateFrictionPercent(frictionDirection, velocity, ratio);
        }

        else {
            velocity = input;
        }

        playerRigidbody.linearVelocity = velocity;
    }
}