using SpreadInfo;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class ShieldInfo: IEffect {
    public DefenceType Type { get; private set; }
    public float Duration { get; private set; }
    public bool Reflect { get; private set; }

    public (bool, float) ApplyDamage(float power) {

        if (Type == DefenceType.None) return (false,power);

        power = Mathf.Ceil(power);
        if (Type == DefenceType.Time) {

            if (Reflect) {
                MonsterSlider.Instance.GetDamage((int)(power * 0.3f));
                return (true, 0);
            }
            power = Mathf.Ceil(power * (Duration - (int)Duration));
            return (true, power);
        }

        if (power < Duration) {

            if (Reflect) MonsterSlider.Instance.GetDamage((int)(power * 0.3f));
            Duration -= power;
            return (true, 0);
        }

        if (Reflect) MonsterSlider.Instance.GetDamage((int)(Duration * 0.3f));
        power -= (int)Duration;
        Type = DefenceType.None;
        Duration = 0;
        Reflect = false;

        return (true, power);
    }

    public void TurnUpdate() {

        if (Type != DefenceType.Time) return;

        Duration--;
        if (Duration >= 1) return; 
        
        Type = DefenceType.None;
        Duration = 0;
        Reflect = false;
    }

    public ShieldInfo(DefenceType type, float power, bool reflect) {
        Type = type;
        
        Duration = power;
        if (Type == DefenceType.Time) Duration++;
        
        Reflect = reflect;
    }
}