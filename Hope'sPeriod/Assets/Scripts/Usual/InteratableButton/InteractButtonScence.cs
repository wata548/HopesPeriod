

using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class InteractButtonScence : InteractButton {
    void CheckClicK() {

        if (!isMouseOn)
            return;
        
        if (Input.GetMouseButton(0)) {
            Click();
        }
    }

    private void Update() {
        CheckClicK();
    }
}