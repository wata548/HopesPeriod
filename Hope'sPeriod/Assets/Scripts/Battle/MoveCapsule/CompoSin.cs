using UnityEngine; 
public sealed class CompoSin: MoveComposite {
    public CompoSin(GameObject owner = null) : base(owner) {}

    public override int Priority { get; protected set; } = 50;
    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 3f;
    private float cycle = 1;
    public override GameObject Owner { get; protected set; }
    
    private float timer = 0;
    private const float PI = 3.141592f; 
    
    public void SetCycle(float value) => cycle = value;

    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo = Direction.None) {

        timer += Time.deltaTime;
        return nextVelo + new Vector2(nextVelo.y, -nextVelo.x).normalized * (Mathf.Cos(timer * PI * cycle) * Power); 
    }
}