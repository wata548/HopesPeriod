using System;
using UnityEngine;

public class PlayerGravity
{

    const float GRAVITY_POWER = 10f;

    static float gravityScale = 0;
    public static Vector2 CalculateGravity(Direction gravityDirection) {

        bool right = DirectionInfo.Contain(gravityDirection, Direction.RIGHT);
        bool left = DirectionInfo.Contain(gravityDirection, Direction.LEFT);
        bool up = DirectionInfo.Contain(gravityDirection, Direction.UP);
        bool down = DirectionInfo.Contain(gravityDirection, Direction.DOWN);

        if ( (left && right) || (up && down)) {

            throw new Exception("This Direction is not correct");
        }


        Vector2 result = Vector2.zero;
        gravityScale += GRAVITY_POWER * Time.deltaTime;
        
        //* It check player's velocity influenced by gravity
        bool check = false;

        foreach(Direction checkDirection in Enum.GetValues(typeof(Direction))) {

            if(!DirectionInfo.Contain(gravityDirection, checkDirection)) {

                continue;
            }
            
            if(!ContactInfo.Check(checkDirection)) {

                result += DirectionInfo.ConvertVector(checkDirection) * gravityScale;
                check = true;
            }

        }

        if(!check) {

            gravityScale = 0;
        }


        return result;
    }

}
