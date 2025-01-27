using SpreadInfo;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class ShieldInfo {
    public DefenceType Type { get; private set; }
    public float Power { get; private set; }
    public bool Reflect { get; private set; }

    public (bool, float) ApplyDamage(float power) {

        if (Type == DefenceType.None) return (false,power);

        power = Mathf.Ceil(power);
        if (Type == DefenceType.Time) {

            if (Reflect) {
                MonsterSlider.Instance.GetDamage((int)(power * 0.3f));
                return (true, 0);
            }
            power = Mathf.Ceil(power * (Power - (int)Power));
            return (true, power);
        }

        if (power < Power) {

            if (Reflect) MonsterSlider.Instance.GetDamage((int)(power * 0.3f));
            Power -= power;
            return (true, 0);
        }

        if (Reflect) MonsterSlider.Instance.GetDamage((int)(Power * 0.3f));
        power -= (int)Power;
        Type = DefenceType.None;
        Power = 0;
        Reflect = false;

        return (true, power);
    }

    public void TurnUpdate() {

        if (Type != DefenceType.Time) return;

        Power--;
        if (Power >= 1) return; 
        
        Type = DefenceType.None;
        Power = 0;
        Reflect = false;
    }

    public ShieldInfo(DefenceType type, float power, bool reflect) {
        Type = type;
        
        Power = power;
        if (Type == DefenceType.Time) Power++;
        
        Reflect = reflect;
    }
}