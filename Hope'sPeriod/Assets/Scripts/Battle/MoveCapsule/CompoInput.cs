using System;
using UnityEngine;

public sealed class CompoInput : MoveComposite {
    
    public override int Priority { get; protected set; } = 0;
    public override Direction Apply { get; set; } = DirectionInfo.All;
    public override float Power { get; set; } = 5;

    //This function be not impacted by past or current velocity
    public override Vector2 Play(Vector2 beforeVelo, Vector2 currentVelo) {
     
        Direction movementDirection = Direction.None;
            
        foreach(Direction checkDirectrion in Enum.GetValues(typeof(Direction))) {
            
            if(checkDirectrion == Direction.None) {
                continue;
            }
            
            //TODO: Contact Check should not static class 
            //* If player contact on wall, Don't need calculate power
            if(ContactInfo.Check(checkDirectrion)) {
            
                continue;
            }
            
            //* Find key and check input
            var checkKey = DirectionInfo.MatchKey(checkDirectrion);
            
            var isClick = InputManager.Instance.Pressing(checkKey);
            
            if (isClick) {
                movementDirection |= checkDirectrion;
            }
        }
        movementDirection &= Apply;
            
        var result = DirectionInfo.ConvertVector(movementDirection);
        result = result.normalized * Power;
            
        return result;
    }
    
}