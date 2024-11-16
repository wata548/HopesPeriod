using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Direction {

    UP = 0b1000,
    DOWN = 0b0100,
    LEFT = 0b0010,
    RIGHT = 0b0001
}

class DirectionInfo {

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

                result += DIRRECTION_VECTOR[dir];
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




public class PlayerPhysics : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
