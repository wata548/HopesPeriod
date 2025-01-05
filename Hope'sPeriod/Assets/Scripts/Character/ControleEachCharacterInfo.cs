using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro;

public class ControleEachCharacterInfo: MonoBehaviour {


    [SerializeField] private Slider hp;
    [SerializeField] private Slider mp;
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
        mp.UpdateInfo(CurrentMp / MaximumMp);
        mpChange.text = power.ToString();
        mpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
        return true;
    }
    
    public bool GetDamageable(float damage) => CurrentHp > damage;

    public bool GetDamage(float damage) {

        CurrentHp -= damage;
        
        ShowDamage(damage);
        if (CurrentHp >= 0) {

            hp.UpdateInfo(CurrentHp / MaximumHp);
            return true;
        }

        hp.UpdateInfo(0);
        
        CurrentHp = 0;
        Dead = true;
        return false;
    }
    public bool GetSafeDamage(float damage) {

        if (!GetDamageable(damage)) {
            return false;
        }

        ShowDamage(damage);
        hp.UpdateInfo(CurrentHp / MaximumHp);
        CurrentHp -= damage;
        return true;
    }

    private void ShowDamage(float damage) {

        Debug.Log($"get damage: {damage}");
        hpChange.text = damage.ToString();
        hpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
}