using UnityEngine;
using System;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;

public sealed class CompoFriction : MoveComposite {
        
    public override int Priority { get; protected set; } = 7;
    public override Direction Apply { get; set; } = DirectionInfo.All;
    public override float Power { get; set; } = 20f;
    public override GameObject Owner { get; protected set; }

    public float trashhold = 0.1f;

    public CompoFriction(GameObject owner) : base(owner) {}
    
    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo) {

        //check state
        if (!UsualVector.Approximately(nextVelo, Vector2.zero)) {

            return nextVelo;
        }
            

        if (UsualVector.Approximately(currentVelo, Vector2.zero))
            return Vector2.zero;
        
        //apply
        float frictionPower = Power * Time.deltaTime;
        frictionPower = Mathf.Abs(frictionPower);
        
        foreach(Direction checkDirection in Enum.GetValues(typeof(Direction))) {
            
            if(!Apply.Contain(checkDirection)) {
            
                continue;
            }
            
            var checkDirectionVector = checkDirection.ConvertVector(); 
            
            int symbol = 0;
            ref float checkVelocityAxis = ref currentVelo.x;
            
            //* Check checkDirection's symbol and save axis info
            if(DirectionInfo.Horizontal.Contain(checkDirection)) {
                symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.x));
            
                checkVelocityAxis = ref currentVelo.x;
            }
            
            else {
                symbol = Convert.ToInt32(Mathf.Sign(checkDirectionVector.y));
            
                checkVelocityAxis = ref currentVelo.y;
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
            
        return currentVelo;
    }
}