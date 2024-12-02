using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class ControleEachCharacterInfo {
    
   //==================================================| Properties and Fields 
    public float MaximumHp { get; private set; }
    public float CurrentHp { get; private set; }
    
    public float MaximumMp { get; private set; }
    public float CurrentMp { get; private set; }

    public float Attract { get; private set; } = 0;
    private Queue<float> rememberAttract = new();
    private readonly int rememberCount = 5;

    public float AlphaAttract { get; private set; } = 0; 
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
        return true;
    }
    
    public bool GetDamageable(float damage) => CurrentHp > damage;

    public bool GetDamage(float damage) {

        if (!GetDamageable(damage)) {
            return false;
        }

        CurrentHp -= damage;
        return true;
    }
}