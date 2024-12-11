using UnityEditor.SceneManagement;
using UnityEngine;


public abstract class BasicMovement : MonoBehaviour {
   
    public abstract void Move();
   
    public virtual Vector2 LinearMove(Vector2 startPos, Vector2 endPos, float speed) {

        var deltaVelocity = LinearMoveVelo(startPos, endPos, speed);
        return startPos + deltaVelocity;
    }

    public virtual Vector2 LinearMoveVelo(Vector2 startPos, Vector2 endPos, float speed) {

        var dis = (startPos - endPos).Distance();
        
        Vector2 deltaVelocity = UsualVector.Inclination(startPos, endPos);
        deltaVelocity = deltaVelocity.normalized;
        deltaVelocity *= speed;
        
        
        return deltaVelocity;
    }
  
    public virtual Vector2 Acceleration(Vector2 startPos, Vector2 endPos, float ratio) {
      
        Vector2 dPos = Vector2.Lerp(startPos, endPos, ratio);

        return dPos;
    }
   
      
}