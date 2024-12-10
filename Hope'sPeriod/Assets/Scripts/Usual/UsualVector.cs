using System;
using UnityEngine;

public static class UsualVector
{
    public static bool Approximately(Vector2 v1, Vector2 v2) {
        bool xAxisCheck = Mathf.Approximately(v1.x, v2.x);
        bool yAxisCheck = Mathf.Approximately(v1.y, v2.y);
        
        return xAxisCheck && yAxisCheck;
    }

    public static Vector2 Inclination(Vector2 startPos, Vector2 endPos) { 
        
        float dx = endPos.x - startPos.x;
        float dy = endPos.y - startPos.y;
       
        Vector2 inclination = new Vector2(dx, dy);
        inclination = inclination.normalized;
        Debug.Log(inclination);

        return inclination;
    }

    public static Vector2Int ToVector2Int(this Vector3 v) {

        var x = Convert.ToInt32(v.x);
        var y = Convert.ToInt32(v.y);

        return new Vector2Int(x, y);
    }

    public static Vector3Int ToVector3Int(this Vector3 v) {
        
        var x = Convert.ToInt32(v.x);
        var y = Convert.ToInt32(v.y);
        var z = Convert.ToInt32(v.z);
        
        return new Vector3Int(x, y, z);
    }

    public static Vector2 ToVec2(this Vector3 vector) => new(vector.x, vector.y);
    public static Vector3 ToVec3(this Vector2 vector) => new(vector.x, vector.y, 0);
}
