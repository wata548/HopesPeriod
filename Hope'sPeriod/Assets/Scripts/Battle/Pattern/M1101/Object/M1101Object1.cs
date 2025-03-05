using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class M1101Object1: BaseEnemy {
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; }
    public override float Damage { get; protected set; }
    private Rigidbody2D rigidbody2D = null;
    
    private const float WidthLength = 6.5f;
    private const float HeightLength = 4f;
   
    public void SetPos(Vector2 pos) {
        MoveComposite.GetComposite<CompoLinear>()
            .SetTarget(pos);
    }

    public void SetAmplificationPower(float power)
        => MoveComposite.SetPower<CompoSin>(power);

    public void SetSpeed(float power)
        => MoveComposite.SetPower<CompoLinear>(power);

    public void SetCycle(float power)
        => MoveComposite.GetComposite<CompoSin>()
            .SetCycle(power);
    
    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        
        ContactStrategy = new TriggerType();
        MoveComposite = new CompositeGroupBase(gameObject)
            .AddComposite(new CompoLinear())
            .AddComposite(new CompoSin());
    }
    
    public override void Move() {

        bool overX = transform.localPosition.x is > WidthLength or < -WidthLength; 
        bool overY = transform.localPosition.y is > HeightLength or < -HeightLength;
        if (overX || overY)
            Destroy(gameObject);
        
        rigidbody2D.linearVelocity = MoveComposite.Play(rigidbody2D.linearVelocity, Vector2.zero);
    }
}