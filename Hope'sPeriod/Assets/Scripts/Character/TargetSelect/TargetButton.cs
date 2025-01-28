using UnityEngine;

public class TargetButton :InteractButtonScence {
    public override void Click() {

        Debug.Log($"fdgdfg {Index}");
        TargetButtonManager.Instance.TurnOff();
    }
}