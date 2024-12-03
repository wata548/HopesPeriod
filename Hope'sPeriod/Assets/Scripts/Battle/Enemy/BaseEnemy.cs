using System;
using UnityEngine;

public abstract class BaseEnemy: BasicMovement, IDamageAble {

    public float Damage { get; private set; }

    public virtual void GiveDamage() {

        ControleCharacterInfo.Instance.DamageDistribute(Damage);
    }
    public abstract override void Move();
}