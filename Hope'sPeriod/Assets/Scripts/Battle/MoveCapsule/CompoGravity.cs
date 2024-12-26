using System;
using UnityEngine;

public sealed class CompoGravity : MoveComposite {
    
    public override int Priority { get; protected set; } = 10;
    public override Direction Apply { get; set; } = Direction.Down;
    public override float Power { get; set; } = 10;

    private float gravityScale = 0;


    public override Vector2 Play(Vector2 beforeVelo, Vector2 currentVelo) {
     
        if (DirectionInfo.ContainOpposite(Apply)) {
        
            throw new Exception("This Direction is not correct");
        }
        
        var result = Vector2.zero;
        gravityScale += Power * Time.deltaTime;
                
        //* It check player's velocity influenced by gravity
        var check = false;
        
        foreach(Direction checkDirection in Enum.GetValues(typeof(Direction))) {
        
            if(!DirectionInfo.Contain(Apply, checkDirection)) 
                continue;
        
            if (ContactInfo.Check(checkDirection)) 
                continue;
        
            result += DirectionInfo.ConvertVector(checkDirection) * gravityScale;
            check = true;
        
        }
        
        if(!check) {
        
            gravityScale = 0;
        }
        
        return currentVelo + result;
    }
    
}