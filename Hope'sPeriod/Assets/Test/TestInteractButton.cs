using UnityEngine;

public class TestInteractButton : InteractButtonUI {
    public override void Click() {
        Debug.Log(gameObject.name);
    }
}
