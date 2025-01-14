using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro;

public class ControleEachCharacterInfo: MonoBehaviour {


    [SerializeField] private CharacterSlider hp;
    [SerializeField] private CharacterSlider mp;
    [SerializeField] private TMP_Text hpChange;
    [SerializeField] private TMP_Text mpChange;
    
    //==================================================| Properties and Fields 

    public float MaximumHp { get; private set; } = 100;
    public float CurrentHp { get; private set; } = 100;

    public float MaximumMp { get; private set; }
    public float CurrentMp { get; private set; }
    public bool Dead { get; private set; }

    public float Attract { get; private set; } = 0;
    
    private Queue<float> rememberAttract = new();
    private readonly int rememberCount = 5;

    public float SkillAttract{ get; private set; } = 0; 
    //==================================================| Method 
    public void AddAbstract(float power) {

        if (rememberAttract.Count >= rememberCount) {
           
            Attract -= rememberAttract.Dequeue();
        }

        rememberAttract.Enqueue(power);
        Attract += power;
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

    public bool GetDamage(float damage) {

        CurrentHp -= damage;
        
        if (CurrentHp >= 0) {

            ShowDamage(damage);
            return true;
        }

        ShowDamage(damage);
        Debug.Log("character dead");
        
        CurrentHp = 0;
        Dead = true;
        return false;
    }

    public bool HealHp(float power, bool revive = false) {
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
    public bool GetSafeDamage(float damage) {

        if (!GetDamageable(damage)) {
            return false;
        }

        ShowDamage(damage);
        CurrentHp -= damage;
        return true;
    }

    private void ShowDamage(float damage) {

        hp.UpdateInfo(CurrentHp, MaximumHp);
        Debug.Log($"Get damage: {damage}");
        hpChange.text = damage.ToString();
        hpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
    
    private void ShowHealHp(float power) {
        
        hp.UpdateInfo(CurrentHp, MaximumHp);
        Debug.Log($"Heal hp: {power}");
        hpChange.text = power.ToString();
        hpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
    private void ShowUseMp(float power) {
    
        mp.UpdateInfo(CurrentMp, MaximumMp);
        Debug.Log($"Use Mp: {power}");
        mpChange.text = power.ToString();
        mpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
    private void ShowHealMp(float power) {

        mp.UpdateInfo(CurrentMp, MaximumMp);
        Debug.Log($"Heal mp: {power}");
        mpChange.text = power.ToString();
        mpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
}