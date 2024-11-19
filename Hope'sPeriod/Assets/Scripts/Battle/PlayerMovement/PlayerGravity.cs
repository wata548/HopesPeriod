using System;
using UnityEngine;

public class PlayerGravity
{

    static float gravityPower = 10f;

    static float gravityScale = 0;

    public static void SetPower(float power) {

        gravityPower = power;
    }

    public static Vector2 CalculateGravity(Direction gravityDirection) {

        if (DirectionInfo.ContainOpposite(gravityDirection)) {

            throw new Exception("This Direction is not correct");
        }


        Vector2 result = Vector2.zero;
        gravityScale += gravityPower * Time.deltaTime;
        
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