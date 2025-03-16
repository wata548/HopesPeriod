using System;
using System.Numerics;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine; 
using Vector2 = UnityEngine.Vector2;

public class PlayerAnimation: MonoBehaviour {
    private Animator animator = null;
    private Vector2 direction;
    public Direction Dir { get; private set; }
    private MoveComposite input;

    private bool isWalking = false; 
    private bool on = false;
    
//==================================================||
    public void SetOn(bool on) => this.on = on;

    private EventInstance walkSound;
    private void Awake() {

        animator = GetComponent<Animator>();
        input = new CompoInput(gameObject);
        walkSound = AudioManager.Instance.CreateInstance(
            FmodEvents.Instance.Walk);
    }

    private void Update() {

        if (!on) return;
        var inputDirection = input.Play(Vector2.zero, Vector2.zero, Direction.None);

        Sound(inputDirection);
        if(!inputDirection.Approximately(Vector2.zero))
            SetAnimation(inputDirection);
    }
    
   //==================================================|| 

    private void Sound(Vector2 velo) {
        if (!isWalking)
            return;
        
        if (velo.magnitude >= 0.1f) {
            bool isPlaying = AudioManager.Instance.IsPlaying(walkSound);
            if (!isPlaying) {
                walkSound.start();
            }
        }
        else {
            walkSound.stop(STOP_MODE.ALLOWFADEOUT);
        }
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

        var speed = power.magnitude;
        isWalking = speed > 0.1f;
        animator.SetFloat("Speed", speed);
    }
}