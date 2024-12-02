using System;
using UnityEngine;

public abstract class BaseMonster: BasicMovement, IDamageAble {

    public float Damage { get; private set; }
    public abstract override void Move();
}