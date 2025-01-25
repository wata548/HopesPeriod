using System;
using SpreadInfo;
using System.Collections.Generic;

public class EffectInfo {

    public static Dictionary<EffectType, string> MatchKorean = new() {
        { EffectType.Slow, "슬로우" },
        { EffectType.AttackUp, "공격 업" }
    };
     
    public EffectType Type { get; private set; }
    public float Power { get; private set; }
    public int Duration { get; private set; }

    public EffectInfo(EffectType type, float power, int duration) {
        Type = type;
        Power = power;
        Duration = duration;
    }

    public EffectInfo(int code) {
        Type = ItemInfo.Effect(code);
        Power = ItemInfo.EffectPower(code);
        Duration = ItemInfo.EffectDuration(code);
    }
        
    public void TurnUpdate() {
               
        if (Duration <= 0) {

            Init();
            return;
        }

        Duration--;
        if (Duration <= 0) Init();
    }

    private void Init() {
        Type = EffectType.None;
        Power = 0;
        Duration = 0;
    }
}