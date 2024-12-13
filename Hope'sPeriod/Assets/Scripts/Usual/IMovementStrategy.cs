using System.Numerics;
using Vector2 = UnityEngine.Vector2;

public interface IMovementStrategy{
    
    Vector2 Move(Vector2 _, Vector2 __, float ___);
    Vector2 Velocity(Vector2 _, Vector2 __, float ___);
}
