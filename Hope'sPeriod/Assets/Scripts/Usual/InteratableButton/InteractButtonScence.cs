

using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class InteractButtonScence : InteractButton {
    void CheckClick() {

        if (!Manager.Interactable)
            return;
        
        if (!isMouseOn)
            return;
        
        if (Input.GetMouseButtonDown(0)) {
            Click();
        }
    }

    private void Update() {
        CheckClick();
    }
}