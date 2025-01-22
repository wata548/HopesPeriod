using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using SpreadInfo;
using TMPro;

public class ControleEachCharacterInfo: MonoBehaviour {


    //==================================================||Set Inspector 
    
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
    
    //==================================================| Properties  

    public float MaximumHp { get; private set; } = 100;
    public float CurrentHp { get; private set; } = 100;
    public float MaximumMp { get; private set; } = 100;
    public float CurrentMp { get; private set; } = 100;
    public DefenceType Shield { get; private set; } = DefenceType.None;
    public float ShieldPower { get; private set; } = 0;
    public bool Dead { get; private set; }
    public float Attract { get; private set; } = 0;

    //==================================================| Method 

    public void SetShield(DefenceType type, float power) {

        Shield = type;
        ShieldPower = power;
    }

    public void UpdateShield() {
        if (Shield != DefenceType.Time) return;
        ShieldPower--;
        if (ShieldPower >= 1) return;
        
        ShieldPower = 0;
        Shield = DefenceType.None;
    }

    public float ApplyShield(float damage) {

        return Shield switch {
            DefenceType.None => damage,
            DefenceType.Time => (ShieldPower - (int)ShieldPower) * damage,
            DefenceType.Break => BreakShieldProcedure(damage)
            
        };

        float BreakShieldProcedure(float damage) {
            if (ShieldPower >= damage) {
                ShieldPower -= damage;
                damage = 0;
            }
            else {
                damage -= ShieldPower;
                ShieldPower = 0;
                Shield = DefenceType.None;
                            
            }

            return damage;
        }
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
}