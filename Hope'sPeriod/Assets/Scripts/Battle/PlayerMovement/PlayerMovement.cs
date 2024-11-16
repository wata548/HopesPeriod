using Mono.Cecil.Cil;
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

    Vector2 CalculateFrictionPercent(Direction frictionDirection, Vector2 velocity, float fritionPower, float trashhold) {

        fritionPower = Mathf.Abs(fritionPower);

        foreach(Direction checkDirection in Enum.GetValues(typeof(Direction))) {

            if(!DirectionInfo.Contain(frictionDirection, checkDirection)) {

                continue;
            }

            Vector2 checkDirectionVector = DirectionInfo.ConvertVector(checkDirection); 

            int symbol = 0;
            ref float checkVelocitysAxis = ref velocity.x;

            //* Check checkDirection's symbol and save axis info
            if(checkDirection >= Direction.LEFT) {

                //* I can't decide between those. but many people say second is more well. (I can't admit it)
                //symbol = (checkDirectionVector.x >= 0 ? 1 : -1);
                symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.x));

                checkVelocitysAxis = ref velocity.x;
            }

            else {

                symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.y));

                checkVelocitysAxis = velocity.y;
            }

            //* calculate friction
            if (symbol * checkVelocitysAxis > 0) {

                if (checkVelocitysAxis <= trashhold) {

                    checkVelocitysAxis = 0;
                }

                else {

                    checkVelocitysAxis *= fritionPower;
                }
            }
        }

        return velocity;
    }

}
