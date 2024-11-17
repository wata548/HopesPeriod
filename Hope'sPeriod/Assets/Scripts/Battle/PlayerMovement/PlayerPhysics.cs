using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[Flags]
public enum Direction {

    NONE = 0b0000,
    UP = 0b1000,
    DOWN = 0b0100,
    LEFT = 0b0010,
    RIGHT = 0b0001
}

//* static class
class DirectionInfo {

    public const Direction NONE = 0b0000;
    public const Direction ALL =  (Direction)0b1111;

    private static readonly Dictionary<Direction, Direction> OPPOSITE_DIRECTION = new() {

        {Direction.UP, Direction.DOWN},
        {Direction.DOWN, Direction.UP},
        {Direction.LEFT, Direction.RIGHT},
        {Direction.RIGHT, Direction.LEFT}
    };

    private static readonly Dictionary<Direction, Vector2> DIRRECTION_VECTOR = new() {

        {Direction.UP, Vector2.up},
        {Direction.DOWN, Vector2.down},
        {Direction.LEFT, Vector2.left},
        {Direction.RIGHT, Vector2.right}
    };

    private static readonly Dictionary<Direction, KeyTypes> MATCH_DIRECTION_KEY = new() {

        { Direction.UP, KeyTypes.UP },
        { Direction.DOWN, KeyTypes.DOWN },
        { Direction.LEFT, KeyTypes.LEFT },
        { Direction.RIGHT, KeyTypes.RIGHT },

    };

    public static bool Contain(Direction target, Direction check) {

        return Convert.ToByte(target & check) != 0;
    }

    public static Vector2 ConvertVector(Direction dir) {

        Vector2 result = Vector2.zero;

        foreach(Direction checkDirection in Enum.GetValues(typeof(Direction))) {

            if(Contain(dir, checkDirection)) {

                result += DIRRECTION_VECTOR[checkDirection];
            }

        }
            
        return result;
    }

    public static Direction OppositeDirection(Direction dir) {

        Direction result = 0;

        foreach(Direction checkDirection in Enum.GetValues(typeof(Direction))) {

            if (Contain(dir, checkDirection)) {

                result |= OPPOSITE_DIRECTION[dir];
            }
        }

        return result;
    }

    public static KeyTypes MatchKey(Direction dir) {
        
        if(!MATCH_DIRECTION_KEY.ContainsKey(dir)) {
            throw new Exception("This dirction didn't correspond to any key");
        }

        return MATCH_DIRECTION_KEY[dir];
    }
}



[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysics : MonoBehaviour
{
    Rigidbody2D playerRigidbody = null;


    private void Awake() {

        if (playerRigidbody == null) {

            playerRigidbody = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        playerRigidbody.linearVelocity = PlayerMovement.CalculateDirection(DirectionInfo.ALL);
    }
}
