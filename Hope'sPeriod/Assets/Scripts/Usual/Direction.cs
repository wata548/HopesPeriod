using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[Flags]
public enum Direction {

    None    = 0b0000,
    Up      = 0b0001,
    Down    = 0b0010,
    Left    = 0b0100,
    Right   = 0b1000
}

//* static class
class DirectionInfo {

    public const Direction All          = (Direction)0b1111;
    public const Direction Vertical     = (Direction)0b0011;
    public const Direction Horizontal   = (Direction)0b1100;

    struct DirectionInfos {

        public Direction    opposite    { get; private set; }
        public Vector2      vector      { get; private set; }
        public KeyTypes     key         { get; private set; }

        public DirectionInfos(Direction opposite, Vector2 vector, KeyTypes key) {

            this.opposite = opposite;
            this.vector = vector;
            this.key = key;
        }
    }

    private static readonly Dictionary<Direction, DirectionInfos> matchDirectionInfos = new() {

        {Direction.Up,      new(Direction.Down,     Vector2.up,     KeyTypes.UP)},
        {Direction.Down,    new(Direction.Up,       Vector2.down,   KeyTypes.DOWN)},
        {Direction.Left,    new(Direction.Right,    Vector2.left,   KeyTypes.LEFT)},
        {Direction.Right,   new(Direction.Left,     Vector2.right,  KeyTypes.RIGHT)}
    };

    public static bool Contain(Direction standard, Direction check) {

        return Convert.ToByte(standard & check) != 0;
    }

    public static bool ContainOpposite(Direction dir) {

        if(Contain(dir, Direction.Left) && Contain(dir, Direction.Right)) {
            return true;
        }
        if (Contain(dir, Direction.Up) && Contain(dir, Direction.Down)) {
            return true;
        }

        return false;
    }

    public static Vector2 ConvertVector(Direction dir) {

        Vector2 result = Vector2.zero;

        foreach (Direction checkDirection in Enum.GetValues(typeof(Direction))) {

            if (Contain(dir, checkDirection)) {

                result += matchDirectionInfos[checkDirection].vector;
            }

        }

        return result;
    }

    public static Direction OppositeDirection(Direction dir) {

        Direction result = 0;

        foreach (Direction checkDirection in Enum.GetValues(typeof(Direction))) {

            if (Contain(dir, checkDirection)) {

                result |= matchDirectionInfos[dir].opposite;
            }
        }

        return result;
    }

    public static KeyTypes MatchKey(Direction dir) {

        if (!matchDirectionInfos.ContainsKey(dir)) {
            throw new Exception("This dirction didn't correspond to any key");
        }

        return matchDirectionInfos[dir].key;
    }
}