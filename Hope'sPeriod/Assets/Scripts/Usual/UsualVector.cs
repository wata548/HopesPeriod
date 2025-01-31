using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public static class UsualVector
{
    public static bool Approximately(this Vector2 v1, Vector2 v2) {
        bool isXApproximately = Mathf.Approximately(v1.x, v2.x);
        bool isYApproximately = Mathf.Approximately(v1.y, v2.y);
        
        return isXApproximately && isYApproximately;
    }

    public static float InnerProduction(this Vector2 v1, Vector2 v2) {
        return v1.x * v2.x + v1.y * v2.y;
    }

    public static float GetDegree(this Vector2 v) {
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public static Vector2 GetVector(this float degree) {
        
        return new(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));
    }
    public static float Distance(this Vector2 v) {
        return v.magnitude;
    }
    public static Vector2 Abs(this Vector2 v) {
        return new(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }

    public static Vector3 Abs(this Vector3 v) {
        return new(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }
    public static Vector2 Inclination(this Vector2 startPos, Vector2 endPos) { 
        
        float dx = endPos.x - startPos.x;
        float dy = endPos.y - startPos.y;
       
        Vector2 inclination = new Vector2(dx, dy);

        return inclination;
    }

    public static Vector2Int ToVec2Int(this Vector3 v) {

        var x = (int)(Mathf.Floor(v.x));
        var y = (int)(Mathf.Floor(v.x));

        return new Vector2Int(x, y);
    }

    public static Vector3Int ToVec3Int(this Vector3 v) {
        
        var x = (int)(Mathf.Floor(v.x));
        var y = (int)(Mathf.Floor(v.y));
        var z = (int)(Mathf.Floor(v.z));
        
        return new Vector3Int(x, y, z);
    }

    public static Vector2Int ToVec2Int(this Vector2 v) => new((int)(Mathf.Floor(v.x)), (int)(Mathf.Floor(v.y)));
    
    public static Vector2 ToVec2(this Vector3 vector) => new(vector.x, vector.y);
    public static Vector2 ToVec2(this Vector2Int v) => new((float)v.x, (float)v.y);
    public static Vector3 ToVec3(this Vector2 vector) => new(vector.x, vector.y, 0);
    public static Vector3 ToVec3(this Vector2 vector, float zCoor) => new(vector.x, vector.y, zCoor);
    public static Vector2 Add(this Vector2 vector, float a) => new Vector2(vector.x + a, vector.y + a);
}
