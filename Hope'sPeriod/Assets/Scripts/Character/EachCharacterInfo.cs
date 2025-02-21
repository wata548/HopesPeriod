using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EachCharacterInfo: MonoBehaviour {

    public const int SkillCountLimit = 4;
    private List<int> skill;
    public List<int> HaveSkills { get; private set; } = new();
    
    //==================================================| Properties  

    public bool Exist { get; private set; } = true;
    public float MaximumHp { get; private set; } = 100;
    public float CurrentHp { get; private set; } = 100;
    public float MaximumMp { get; private set; } = 100;
    public float CurrentMp { get; private set; } = 50;
    private bool dead;
    public bool Dead {
        get {
            dead = CurrentHp == 0;
            return dead;
        }
        private set {
            dead = value;
        }
    }

    //==================================================| Method 
    public void SetSkill(int index, int code) {
        if (index >= SkillCountLimit) {
            throw new OutOfRange(0, SkillCountLimit - 1, index);
        }

        for (int i = 0; i < skill.Count; i++) {
            if (skill[i] == code) {
                skill[i] = skill[index];
                skill[index] = code;
                return;
            }
        }
        skill[index] = code;
    }
    public int GetSkill(int index) {

        if (index >= SkillCountLimit) {
            throw new OutOfRange(0, SkillCountLimit - 1, index);
        }

        return skill[index];
    }

    public List<int> Skill => skill;

    public void LockOffSkill(int code) {
        if (code.ToCodeType() == CodeType.Skill) {
            if (!HaveSkills.Any(skill => skill == code)) {
                HaveSkills.Add(code);
            }
        }
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
        if (CurrentMp > MaximumMp)
            CurrentMp = MaximumMp;
                        
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
        Destroy(Monster.Instance.gameObject);
                
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

    public void Load(SaveCharacterInfo info) {
        Exist = info.Exist;
        Dead = info.Dead;
        skill = info.Skill.ToList();
        while (skill.Count < SkillCountLimit) {
            skill.Add(0);
        }
        HaveSkills = info.HaveSkill.ToList();

        MaximumHp = info.MaximumHp;
        CurrentHp = info.CurrentHp;
        MaximumMp = info.MaximumMp;
        CurrentMp = info.CurrentMp;
    }
    
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}