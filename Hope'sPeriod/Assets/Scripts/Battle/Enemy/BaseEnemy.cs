using System;
using UnityEngine;

public abstract class BaseEnemy: BasicMovement, IDamageAble {

    public float Damage { get; private set; }

    public abstract override void Move();
    public abstract float ContactPlayer();
    
}