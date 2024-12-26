using UnityEngine;
using System;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;

public sealed class CompoFriction : MoveComposite {
        
    public override int Priority { get; protected set; } = 7;
    public override Direction Apply { get; set; } = DirectionInfo.All;
    public override float Power { get; set; } = 60f;
    public float Ratio { get; protected set; } = 0.8f;

    public float trashhold = 0.1f;

    public override Vector2 Play(Vector2 beforeVelo, Vector2 currentVelo) {

        //check state
        if (!UsualVector.Approximately(currentVelo, Vector2.zero)) {

            return currentVelo;
        }
            

        if (UsualVector.Approximately(beforeVelo, Vector2.zero))
            return Vector2.zero;
        
        //apply
        float frictionPower = Power * Ratio * Time.deltaTime;
        frictionPower = Mathf.Abs(frictionPower);
        
        foreach(Direction checkDirection in Enum.GetValues(typeof(Direction))) {
            
            if(!DirectionInfo.Contain(Apply, checkDirection)) {
            
                continue;
            }
            
            var checkDirectionVector = DirectionInfo.ConvertVector(checkDirection); 
            
            int symbol = 0;
            ref float checkVelocityAxis = ref beforeVelo.x;
            
            //* Check checkDirection's symbol and save axis info
            if(DirectionInfo.Contain(DirectionInfo.Horizontal, checkDirection)) {
                symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.x));
            
                checkVelocityAxis = ref beforeVelo.x;
            }
            
            else {
                symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.y));
            
                checkVelocityAxis = ref beforeVelo.y;
            }
            
            //* calculate friction
            if (symbol * checkVelocityAxis <= 0)
                continue;
            
            //trashhold
            if (Mathf.Abs(checkVelocityAxis) <= trashhold) {
            
                checkVelocityAxis = 0;
            }
            //apply friction
            else {
                            
                checkVelocityAxis *= frictionPower;
            }
        }
            
        return beforeVelo;
    }
}