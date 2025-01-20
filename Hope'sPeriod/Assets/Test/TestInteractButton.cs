using UnityEngine;

public class TestInteractButton : InteractButtonScence {
    public override void Click() {
        Debug.Log(gameObject.name);
    }
}
