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

            if(DirectionInfo.Contain(frictionDirection, checkDirection)) {

                int symbol = 0;

                Vector2 checkDirectionVector = DirectionInfo.ConvertVector(checkDirection); 

                if(checkDirection >= Direction.LEFT) {

                    //* I can't decide between those. but many people say second is more well. (I can't admit it)
                    //symbol = (checkDirectionVector.x >= 0 ? 1 : -1);
                    symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.x));
                    if(symbol * velocity.x > 0) {

                        if(velocity.x <= trashhold) {

                            velocity.x = 0;
                        }

                        else {

                            velocity.x *= fritionPower;
                        }
                    }
                }

                else {

                    symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.y));
                    if (symbol * velocity.y > 0) {

                        if (velocity.y <= trashhold) {

                            velocity.y = 0;
                        }

                        else {

                            velocity.y *= fritionPower;
                        }
                    }
                }
            }
        }

        return velocity;
    }

}
