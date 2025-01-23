using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using SpreadInfo;
using TMPro;
using UnityEngine.UI;

public class ControleEachCharacterInfo: MonoBehaviour {


    //==================================================||Set Inspector 

    [SerializeField] private SpriteRenderer profile;
    [SerializeField] private CharacterSlider hp;
    [SerializeField] private CharacterSlider mp;
    [SerializeField] private TMP_Text hpChange;
    [SerializeField] private TMP_Text mpChange;
    
    //==================================================||Field 

    private readonly Color healHpColor = Color.green;
    private readonly Color healMpColor = Color.green;
    private readonly Color shieldColor = Color.cyan;
    private readonly Color damageHpColor = Color.white;
    private readonly Color useMpColor = Color.white;
    private readonly Color DeadGray = new(0.3f, 0.3f, 0.3f);
    private const int SkillCountLimit = 4;
    private readonly int[] skill = new int[SkillCountLimit];

    private EffectInfo effectInfo = new EffectInfo(EffectType.None, 0, 0);
    private Color originColor;
    
    //==================================================| Properties  

    public float MaximumHp { get; private set; } = 100;
    public float CurrentHp { get; private set; } = 100;
    public float MaximumMp { get; private set; } = 100;
    public float CurrentMp { get; private set; } = 100;
    public DefenceType Shield { get; private set; } = DefenceType.None;
    public float ShieldPower { get; private set; } = 0;
    public bool Reflection { get; private set; } = false;
    public bool Dead { get; private set; }
    public float Attract { get; private set; } = 0;
    public int AttractDuration { get; private set; }= 0;
    
    //==================================================| Method 

    public void SetAttract(int code) {
        Debug.Log("attract");
        Attract = ItemInfo.Attract(code);
        AttractDuration = ItemInfo.AttractDuration(code);
    }
    
    public void SetAttract(int power, int duration) {
        Attract = power;
        AttractDuration = duration;
    }

    public void SetEffect(int code) => effectInfo = new EffectInfo(code);
    public void SetEffect(EffectInfo info) => effectInfo = info;

    private void SetColor(Color color) {
        hp.ChangeColor(color);
        mp.ChangeColor(color); 
        profile.color = color;
                
    }

    public void SetShield(int code) {
        Shield = ItemInfo.DefenceType(code); 
        ShieldPower = ItemInfo.DefencePower(code);
        Reflection = ItemInfo.DefenceReflect(code);

    }
    public void SetShield(DefenceType type, float power, bool reflect = false) {

        Shield = type;
        ShieldPower = power;
        Reflection = reflect;
    }

    public void TurnUpdate() {
        UpdateAttract();
        UpdateShield();
        effectInfo.TurnUpdate();
    }
    
    private void UpdateShield() {
        if (Shield != DefenceType.Time) return;
        ShieldPower--;
        if (ShieldPower >= 1) return;
        
        ShieldPower = 0;
        Shield = DefenceType.None;
        Reflection = false;
    }

    private void UpdateAttract() {
        if (AttractDuration <= 0) return;
        
        AttractDuration--;
        if (AttractDuration <= 0) {
            Attract = 0;
            AttractDuration = 0;
        }
    } 

    public float ApplyShield(float damage) {

        return Shield switch {
            DefenceType.None => damage,
            DefenceType.Time => TimeShieldProcedure(damage),
            DefenceType.Break => BreakShieldProcedure(damage)
            
        };

        float TimeShieldProcedure(float damage) {

            damage = (ShieldPower - (int)ShieldPower) * damage;
            if (Reflection) {
                //TODO: Attack to Monster
                damage = 0;
            }

            return damage;
        }
        
        float BreakShieldProcedure(float damage) {

            float power = 0;
            
            if (ShieldPower >= damage) {
                if (Reflection) power = damage;
                
                ShieldPower -= damage;
                damage = 0;
            }
            else {
                if (Reflection) power = ShieldPower;
                
                damage -= ShieldPower;
                ShieldPower = 0;
                Shield = DefenceType.None;
                            
            }
            
            //TODO: if power != 0 attack to monster

            return damage;
        }
    }

    public void SetSkill(int index, int code) {
         if (index >= SkillCountLimit) {
                throw new OutOfRange(0, SkillCountLimit - 1, index);
         }

         skill[index] = code;
    }
    
    public int GetSkill(int index) {

        if (index >= SkillCountLimit) {
            throw new OutOfRange(0, SkillCountLimit - 1, index);
        }

        return index;
    }
    
    public bool UseableMp(float power) =>CurrentMp >= power;
    public bool UseMp(float power) {

        if (!UseableMp(power)) {
            return false;
        }

        CurrentMp -= power;

        ShowUseMp(power);
        return true;
    }
    public bool HealMp(float power) {
    
        CurrentMp += power;
        if (CurrentMp > MaximumHp)
            CurrentMp = MaximumHp;
                
        ShowHealMp(power);
        return true;
    }
    public bool GetDamageable(float damage) => CurrentHp > damage;
    public bool GetSafeDamage(float damage) {

        if (!GetDamageable(damage)) {
            return false;
        }

        ShowDamage(damage);
        CurrentHp -= damage;
        return true;
    }
    
    public bool GetDamage(float damage, bool shieldApply = true) {

        if (shieldApply) damage = ApplyShield(damage);
        damage = Mathf.Ceil(damage);
        
        CurrentHp -= damage;
        
        if (CurrentHp > 0) {

            ShowDamage(damage);
            return true;
        }

        CurrentHp = 0;
        ShowDamage(damage);

        StartCoroutine(Wait.WaitAndDo(0.6f, () => SetColor(DeadGray)));
        
        Debug.Log("character dead");
        
        Dead = true;
        return false;
    }

    public bool HealHp(float power, bool revive = false) {

        Debug.Log($"heal Hp {power}");
        
        if (!revive && CurrentHp <= 0)
            return false;

        if (revive && CurrentHp <= 0) {
            //TODO: Revive Event
            StartCoroutine(Wait.WaitAndDo(0.6f, () => SetColor(originColor)));
        }
        
        CurrentHp += power;
                
        if (CurrentHp > MaximumHp) {

            //If release this summary, Player can see how many heal actually
            //power -= CurrentHp - MaximumHp;
            CurrentHp = MaximumHp;
        }
        ShowHealHp(power);
        
        return true;
    }
   
    private void ShowDamage(float damage) {

        hp.UpdateInfo(CurrentHp, MaximumHp);
        
        Debug.Log($"Get damage: {damage}");
        hpChange.text = damage.ToString();
        if (Shield != DefenceType.None) hpChange.color = shieldColor; 
        else hpChange.color = damageHpColor;
        
        hpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
    
    private void ShowHealHp(float power) {
        
        hp.UpdateInfo(CurrentHp, MaximumHp);
        
        Debug.Log($"Heal hp: {power}");
        hpChange.text = power.ToString();
        hpChange.color = healHpColor;
        
        hpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
    private void ShowUseMp(float power) {
    
        mp.UpdateInfo(CurrentMp, MaximumMp);
        
        Debug.Log($"Use Mp: {power}");
        mpChange.text = power.ToString();
        mpChange.color = useMpColor;
        
        mpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
    private void ShowHealMp(float power) {

        mp.UpdateInfo(CurrentMp, MaximumMp);
        
        Debug.Log($"Heal mp: {power}");
        mpChange.text = power.ToString();
        mpChange.color = healMpColor;
        
        mpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }

    private void Awake() {

        originColor = profile.color;
        skill[0] = 9301;
    }
}