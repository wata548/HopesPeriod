using System;
using UnityEngine;

public sealed class CompoInput : MoveComposite {
    
    public override int Priority { get; protected set; } = 0;
    public override Direction Apply { get; set; } = DirectionInfo.All;
    public override float Power { get; set; } = 5;
    public override GameObject Owner { get; protected set; }

    public CompoInput(GameObject owner) : base(owner) {}
    
    //This function be not impacted by past or current velocity
    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo) {
     
        Direction movementDirection = Direction.None;
            
        foreach(Direction checkDirectrion in Enum.GetValues(typeof(Direction))) {
            
            if(checkDirectrion == Direction.None) {
                continue;
            }
            
            //TODO: Contact Check should not static class 
            //* If player contact on wall, Don't need calculate power
            if(contactInfo.Contain(checkDirectrion)) {
            
                continue;
            }
            
            //* Find key and check input
            var checkKey = checkDirectrion.MatchKey();
            
            var isClick = InputManager.Instance.Pressing(checkKey);
            
            if (isClick) {
                movementDirection |= checkDirectrion;
            }
        }
        movementDirection &= Apply;
            
        var result = movementDirection.ConvertVector();
        result = result.normalized * Power;
            
        return result;
    }
    
}