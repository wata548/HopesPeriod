using System;
using DG.Tweening;
using UnityEngine;

public class TestInteractButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; } = true;
    
    private Vector3 size;
    private Tween animation = null;
    
    public override void SelectIn(InteractButton target) {
        Debug.Log("in");
        animation = target.transform.DOScale(size * 1.2f, 0.1f);
    }

    public override void SelectOut(InteractButton target) {
        Debug.Log("out");
        if (animation != null)
            animation.Kill();
        
        target.transform.localScale = size;
    }

    private void Start() {
        size= buttons[0].transform.localScale;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            PriviousButton();
            UpdateState();
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            
            NextButton();
            UpdateState();
        }
    }
}