using UnityEngine;


public abstract class BasicMovement : MonoBehaviour {
   
   public abstract void Move();
   
   public virtual Vector2 LinearMove(Vector2 startPos, Vector2 endPos, float speed) {
         
      Vector2 dVelocity = UsualVector.Inclination(startPos, endPos) * speed;
      return startPos + dVelocity;
   }
  
   public virtual Vector2 Acceleration(Vector2 startPos, Vector2 endPos, float ratio) {
      
      Vector2 dPos = Vector2.Lerp(startPos, endPos, ratio);

      return dPos;
   }
   
      
}
