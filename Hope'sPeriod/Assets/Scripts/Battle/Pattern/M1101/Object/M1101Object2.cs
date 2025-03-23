using System;

public class M1101Object2: LinearPattern {
    public override float Damage { get; protected set; } = 20;

    private void Start() {
        base.speed = 9;
        base.Start();

        /*MoveComposite
            .AddComposite(new CompoSin())
            .SetPower<CompoSin>(10);*/
    }
}