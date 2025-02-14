using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
static class DirectionInfo {

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

        {Direction.Up,      new(Direction.Down,     Vector2.up,     KeyTypes.Up)},
        {Direction.Down,    new(Direction.Up,       Vector2.down,   KeyTypes.Down)},
        {Direction.Left,    new(Direction.Right,    Vector2.left,   KeyTypes.Left)},
        {Direction.Right,   new(Direction.Left,     Vector2.right,  KeyTypes.Right)}
    };

    public static bool Contain(this Direction standard, Direction check) {

        return Convert.ToByte(standard & check) != 0;
    }

    public static bool ContainOpposite(this Direction dir) {

        if(dir.Contain(Direction.Left & Direction.Right)) {
            
            return true;
        }
        if (dir.Contain(Direction.Up & Direction.Down)) {
            return true;
        }

        return false;
    }

    public static float SimpleDirection(this Direction dir) {
        var simple = Enum.GetValues(typeof(Direction)).Cast<Direction>().Any(simple => simple == dir);
        return simple ? 1 : 1.414f;
    }
    
    public static Vector2 ConvertVector(this Direction dir) {

        Vector2 result = Vector2.zero;

        foreach (Direction checkDirection in Enum.GetValues(typeof(Direction))) {

            if (dir.Contain(checkDirection)) {

                result += matchDirectionInfos[checkDirection].vector;
            }

        }

        return result;
    }

    public static Direction OppositeDirection(this Direction dir) {

        Direction result = 0;

        foreach (Direction checkDirection in Enum.GetValues(typeof(Direction))) {

            if (dir.Contain(checkDirection)) {

                result |= matchDirectionInfos[dir].opposite;
            }
        }

        return result;
    }

    public static KeyTypes MatchKey(this Direction dir) {

        if (!matchDirectionInfos.ContainsKey(dir)) {
            throw new Exception("This dirction didn't correspond to any key");
        }

        return matchDirectionInfos[dir].key;
    }
}