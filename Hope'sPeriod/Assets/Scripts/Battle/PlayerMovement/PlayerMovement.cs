using Mono.Cecil.Cil;
using System;
using UnityEngine;

public class PlayerMovement
{
    
    public static Vector2 CalculateDirection(Direction moveableDirection) {

        Direction movementDirection = Direction.None;

        foreach(Direction checkDirectrion in Enum.GetValues(typeof(Direction))) {

            if(checkDirectrion == Direction.None) {

                continue;
            }

            //* If player contact on wall, Don't need calculate power
            if(ContactInfo.Check(checkDirectrion)) {

                continue;
            }

            //* Find key and check input
            var checkKey = DirectionInfo.MatchKey(checkDirectrion);

            var isClick = InputManager.Instance.Pressing(checkKey);

            if (isClick) {
                movementDirection |= checkDirectrion;
            }
        }

        var result = DirectionInfo.ConvertVector(movementDirection);
        result = result.normalized;

        return result;

    }

    public static Vector2 CalculateFrictionPercent(Direction frictionDirection, Vector2 velocity, float fritionPower = 0.5f, float trashhold = 0.01f) {

        fritionPower = Mathf.Abs(fritionPower);

        foreach(Direction checkDirection in Enum.GetValues(typeof(Direction))) {

            if(!DirectionInfo.Contain(frictionDirection, checkDirection)) {

                continue;
            }

            var checkDirectionVector = DirectionInfo.ConvertVector(checkDirection); 

            int symbol = 0;
            ref float checkVelocityAxis = ref velocity.x;

            //* Check checkDirection's symbol and save axis info
            if(DirectionInfo.Contain(DirectionInfo.Horizontal, checkDirection)) {

                //* I can't decide between those. but many people say second is more well. (I can't admit it)
                //symbol = (checkDirectionVector.x >= 0 ? 1 : -1);
                symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.x));

                checkVelocityAxis = ref velocity.x;
            }

            else {

                symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.y));

                checkVelocityAxis = ref velocity.y;
            }

            //* calculate friction
            if (symbol * checkVelocityAxis <= 0)
                continue;

            if (Mathf.Abs(checkVelocityAxis) <= trashhold) {

                checkVelocityAxis = 0;
            }
            else {
                
                checkVelocityAxis *= fritionPower;
            }
        }

        return velocity;
    }

}
