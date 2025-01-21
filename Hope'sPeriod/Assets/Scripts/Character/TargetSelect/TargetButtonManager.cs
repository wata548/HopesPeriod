using System.Security.Cryptography;

public class TargetButtonManager : InteractButtonManager {
    public override bool Interactable { get; set; } = false;
    public override void SelectIn(InteractButton target) {
        SelectCursor.Instance.SetIndex(target.Index);
    }

    public static TargetButtonManager Instance { get; private set; } = null;

    public override void SelectOut(InteractButton target) {}

    public void TurnOn(SelectType type) {
        Interactable = true;
        SelectCursor.Instance.TurnOn(type);
    }

    private void Awake() {
        base.Awake();
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }
}