using System;
using UnityEngine;

public abstract class BaseEnemy: MonoBehaviour, IDamageAble {
    
    public abstract GameObject Player{ get; protected set; }
    public abstract IMovementStrategy MovementStrategy { get; protected set; }
    public abstract ContactStrategy ContactStrategy { get; protected set; }
    public abstract float Damage { get; protected set; }

    public abstract void Move();
    protected virtual void OnCollisionEnter2D(Collision2D other) {
        
        if (other.transform.CompareTag("Player")) {
            
            ContactStrategy.ColliderContact(Player);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        
        if (other.transform.CompareTag("Player")) {
            
            ContactStrategy.TriggerContact(Player);
        }
    }
}