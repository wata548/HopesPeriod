using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class TracingMovement: IMovementStrategy {
    
    public Vector2 Move(Vector2 target, Vector2 pos, float power) {

        var addForce = Velocity(target, pos, power) * Time.deltaTime;
        return target + addForce;
    }

    public Vector2 Velocity(Vector2 target, Vector2 pos, float power) {

        var dis = (pos - target).Distance();

        Vector2 velocity = pos.Inclination(target);
        velocity = velocity.normalized * power;

        return velocity;
    }
}