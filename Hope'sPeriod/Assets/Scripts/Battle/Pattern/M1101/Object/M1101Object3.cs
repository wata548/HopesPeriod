public class M1101Object3: BaseEnemy {
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; } = new TriggerType();
    public override float Damage { get; protected set; } = 26;
    public override void Move() {
    }
}