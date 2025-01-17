public class TargetButtonManager : InteractButtonManager {
    public override bool Interactable { get; set; }
    public override void SelectIn(InteractButton target) {
        SelectCursor.Instance.SetIndex(target.Index);
    }

    public override void SelectOut(InteractButton target) {}
}