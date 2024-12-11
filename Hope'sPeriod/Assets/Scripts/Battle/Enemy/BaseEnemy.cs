using System;
using UnityEngine;

public abstract class BaseEnemy: BasicMovement, IDamageAble {
    
    public abstract ContactStrategy Strategy { get; protected set; }
    public abstract float Damage { get; protected set; }
    public abstract override void Move();
    public abstract GameObject ProcessTarget { get; protected set; }

    protected virtual void OnCollisionEnter2D(Collision2D other) {
        
        if (other.transform.CompareTag("Player")) {
            
            Strategy.ColliderContact(ProcessTarget);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        
        if (other.transform.CompareTag("Player")) {
            
            Strategy.TriggerContact(ProcessTarget);
        }
    }
}