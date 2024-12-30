using UnityEngine;

public sealed class CompoSin: MoveComposite {
    public CompoSin(GameObject owner) : base(owner) {}

    public override int Priority { get; protected set; } = 50;
    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 3f;
    public override GameObject Owner { get; protected set; }
    
    private float timer = 0;
    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo = Direction.None) {
        
        timer += Time.deltaTime;
        return nextVelo + new Vector2(nextVelo.y, -nextVelo.x) * (Mathf.Cos(timer * 10) * Power);
    }
}