using System;
using UnityEngine;

public sealed class CompoGravity : MoveComposite {
    
    public override int Priority { get; protected set; } = 10;
    public override Direction Apply { get; set; } = Direction.Down;
    public override float Power { get; set; } = 10;
    public override GameObject Owner { get; protected set; }

    private float gravityScale = 0;

    public CompoGravity(GameObject owner): base(owner) {}
    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo) {
     
        if (Apply.ContainOpposite()) {
        
            throw new Exception("This Direction is not correct");
        }
        
        var result = Vector2.zero;
        gravityScale += Power * Time.deltaTime;
                
        //* It check player's velocity influenced by gravity
        var check = false;
        
        foreach(Direction checkDirection in Enum.GetValues(typeof(Direction))) {
        
            if(!Apply.Contain(checkDirection)) 
                continue;
        
            if (contactInfo.Contain(checkDirection)) 
                continue;
        
            result += checkDirection.ConvertVector() * gravityScale;
            check = true;
        
        }
        
        if(!check) {
        
            gravityScale = 0;
        }
        
        return nextVelo + result;
    }
    
}