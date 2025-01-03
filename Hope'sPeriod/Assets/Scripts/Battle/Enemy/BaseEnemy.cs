using System;
using UnityEngine;

public abstract class BaseEnemy: MonoBehaviour, IDamageAble {
    
    public static GameObject Player{ get; protected set; }
    public abstract CompositeGroupBase MoveComposite{ get; protected set; }
    public abstract ContactStrategy ContactStrategy { get; protected set; }
    public abstract float Damage { get; protected set; }

    public static void SetPlayer(GameObject player) {
        Player = player; 
    }
    public abstract void Move();
    protected virtual void OnCollisionEnter2D(Collision2D other) {
        
        ContactStrategy.ColliderContact(other.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        
        ContactStrategy.TriggerContact(other.gameObject);
    }

    private void Update() {
        Move();
    }
}