using Unity.Collections.LowLevel.Unsafe;

public class AttractInfo {

    public float Power { get; private set; } = 0;
    public int Duraction { get; private set; } = 0;

    public AttractInfo() {
        Power = 0;
        Duraction = 0;
    }
    
    public AttractInfo(float power, int duraction = 1) {

        Power = power;
        Duraction = duraction + 1;
    }

    public void TurnUpdate() {

        if (Duraction == 0) return;
        
        Duraction--;
        if (Duraction <= 0) {
            Power = 0;
            Duraction = 0;
        }
    }
}