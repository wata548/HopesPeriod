using System;
using UnityEngine;

public class EachCharacterInfo: MonoBehaviour {

    private const int SkillCountLimit = 4;
    private readonly int[] skill = new int[SkillCountLimit];
    
    //==================================================| Properties  

    public bool Exist { get; private set; } = true;
    public float MaximumHp { get; private set; } = 100;
    public float CurrentHp { get; private set; } = 10;
    public float MaximumMp { get; private set; } = 100;
    public float CurrentMp { get; private set; } = 50;
    public bool Dead { get; private set; }
    
    //==================================================| Method 

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

        return skill[index];
    }
    
    public bool UseableMp(float power) =>CurrentMp >= power;
    public bool UseMp(float power) {
        
        if (!UseableMp(power)) {
            return false;
        }
        
        CurrentMp -= power;
        
        return true;
    }
    public bool HealMp(float power) {
            
        CurrentMp += power;
        if (CurrentMp > MaximumHp)
            CurrentMp = MaximumHp;
                        
        return true;
    }
    public bool GetDamageable(float damage) => CurrentHp > damage;
    public bool GetSafeDamage(float damage) {
        
        if (!GetDamageable(damage)) {
            return false;
        }
        
        CurrentHp -= damage;
        return true;
    }
    public bool GetDamage(float damage) {
        
        damage = Mathf.Ceil(damage);
                
        CurrentHp -= damage;
                
        if (CurrentHp > 0) {
        
            return true;
        }
        
        CurrentHp = 0;
        Debug.Log("character dead");
                
        Dead = true;
        return false;
    }
    public bool HealHp(float power, bool revive = false) {
        
        Debug.Log($"heal Hp {power}");
                
        if (!revive && CurrentHp <= 0)
            return false;
        
        if (revive && CurrentHp <= 0) {
            Dead = false;
        }
                
        CurrentHp += power;
                        
        if (CurrentHp > MaximumHp) {
        
            CurrentHp = MaximumHp;
        }
                
        return true;
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}