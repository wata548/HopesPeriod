using System;
using UnityEngine;

public class PlayerMovement
{
    
    Vector2 CalculateDirection(Direction moveableDirection) {

        Direction movementDirection = 0;

        foreach(Direction checkDirectrion in Enum.GetValues(typeof(Direction))) {

            KeyTypes checkKey = DirectionInfo.MatchKey(checkDirectrion);

            bool isClick = InputManager.Instance.Pressing(checkKey);

            if (isClick) {

                moveableDirection |= checkDirectrion;
            }
        }

        Vector2 result = DirectionInfo.ConvertVector(movementDirection);
        result = result.normalized;

        return result;

    }

    Vector2 CalculateConflict(Vector2 velocity, float conflict) {

        velocity *= conflict;

        return velocity;
    }

}
