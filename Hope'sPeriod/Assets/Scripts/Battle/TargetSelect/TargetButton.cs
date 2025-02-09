using UnityEngine;

public class TargetButton :InteractButtonScence {
    public override void Click() {

        TargetButtonManager.Instance.TurnOff();
    }
}