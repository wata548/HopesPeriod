using System;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerAnimation: MonoBehaviour {
    private Animator animator = null;

    private void Awake() {

        animator ??= GetComponent<Animator>();
    }

    public void SetAnimation(Vector2 movement) {

        animator.SetFloat("Speed", movement.magnitude);
        if (movement == Vector2.zero)
            return;
        animator.SetFloat("Horizontal", Mathf.Sign(movement.x));
        if (movement.x == 0) {
            animator.SetFloat("Horizontal", 0);
        }
        animator.SetFloat("Vertical", Mathf.Sign(movement.y));
        if (movement.y == 0) {
            animator.SetFloat("Vertical", 0);
        }
    }
}