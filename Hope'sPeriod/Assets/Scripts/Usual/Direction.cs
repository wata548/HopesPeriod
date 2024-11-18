using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[Flags]
public enum Direction {

    NONE    = 0b0000,
    UP      = 0b0001,
    DOWN    = 0b0010,
    LEFT    = 0b0100,
    RIGHT   = 0b1000
}

//* static class
class DirectionInfo {

    public const Direction NONE         = (Direction)0b0000;
    public const Direction ALL          = (Direction)0b1111;
    public const Direction VERTICAL     = (Direction)0b0011;
    public const Direction HORIZONTAL   = (Direction)0b1100;

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

        {Direction.UP,      new(Direction.UP,       Vector2.up,     KeyTypes.UP)},
        {Direction.DOWN,    new(Direction.DOWN,     Vector2.down,   KeyTypes.DOWN)},
        {Direction.LEFT,    new(Direction.LEFT,     Vector2.left,   KeyTypes.LEFT)},
        {Direction.RIGHT,   new(Direction.RIGHT,    Vector2.right,  KeyTypes.RIGHT)}
    };

    public static bool Contain(Direction standard, Direction check) {

        return Convert.ToByte(standard & check) != 0;
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