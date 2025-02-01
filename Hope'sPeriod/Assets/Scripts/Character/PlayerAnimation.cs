using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine; 
using Vector2 = UnityEngine.Vector2;

public class PlayerAnimation: MonoBehaviour {
    private Animator animator = null;
    private Vector2 direction;
    public Direction Dir { get; private set; }
    private MoveComposite input;
    
    private void Awake() {

        animator ??= GetComponent<Animator>();
        input = new CompoInput(gameObject);
    }

    private void Update() {
        
        var inputDirection = input.Play(Vector2.zero, Vector2.zero, Direction.None);

        if(!inputDirection.Approximately(Vector2.zero))
            SetAnimation(inputDirection);
    }

    private void SetAnimation(Vector2 movement) {

        direction = movement;
        animator.SetFloat("Horizontal", Mathf.Sign(movement.x));
        if (movement.x == 0) {
            direction.x = 0;
            animator.SetFloat("Horizontal",0);
        }
        animator.SetFloat("Vertical", Mathf.Sign(movement.y));
        if (movement.y == 0) {
            direction.y = 0;
            animator.SetFloat("Vertical", 0);
        }

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) 
            Dir = direction.x > 0 ? Direction.Left : Direction.Right;
        else 
            Dir = direction.y > 0 ? Direction.Up : Direction.Down;
    }

    public void SetSpeed(Vector2 power) {
        animator.SetFloat("Speed", power.magnitude);
    }
    
    public void SetSpeed(float power) {
        
        animator.SetFloat("Speed", power);
    }
}