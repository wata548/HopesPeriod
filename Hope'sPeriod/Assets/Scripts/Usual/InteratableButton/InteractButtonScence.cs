

using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class InteractButtonScence : InteractButton {
    void CheckClicK() {

        if (!IsMouseOn)
            return;
        
        if (Input.GetMouseButton(0)) {
            Click();
        }
    }

    private void Update() {
        CheckClicK();
    }
}