using Unity.Collections.LowLevel.Unsafe;

public class AttractInfo: IEffect {

    public float Power { get; private set; } = 0;
    public float Duration { get; private set; } = 0;

    public AttractInfo() {
        Power = 0;
        Duration = 0;
    }
    
    public AttractInfo(float power, int duraction = 1) {

        Power = power;
        Duration = duraction + 1;
    }

    public void TurnUpdate() {

        if (Duration == 0) return;
        
        Duration--;
        if (Duration <= 0) {
            Power = 0;
            Duration = 0;
        }
    }
}