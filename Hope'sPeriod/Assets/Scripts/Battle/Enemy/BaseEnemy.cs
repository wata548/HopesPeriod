using System;
using UnityEngine;

public abstract class BaseEnemy: BasicMovement, IDamageAble {

    public abstract float Damage { get; protected set; }

    public abstract override void Move();
    //public abstract float ContactPlayer();
    
}