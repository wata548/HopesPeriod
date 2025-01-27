using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private readonly Color deadGray = new(0.3f, 0.3f, 0.3f);
    
    private const int SkillCountLimit = 4;
    private readonly int[] skill = new int[SkillCountLimit];

    public List<EffectInfo> Effects { get; private set; } = new();
    public ShieldInfo Shield { get; private set; } = new (DefenceType.None, 0, false);
    public AttractInfo Attract { get; private set; } = new();
    private Color originColor;
    
    //==================================================| Properties  

    public float MaximumHp { get; private set; } = 100;
    public float CurrentHp { get; private set; } = 100;
    public float MaximumMp { get; private set; } = 100;
    public float CurrentMp { get; private set; } = 100;
    public bool Dead { get; private set; }
    
    //==================================================| Method 

    public void SetAttract(int code) {
        Attract = new(ItemInfo.Attract(code),
            ItemInfo.AttractDuration(code));
    }
    
    public void SetAttract(int power, int duration) {
        Attract = new(power, duration);
    }

    public void SetEffect(int code) {

        EffectType type = ItemInfo.Effect(code);
        for (int i = 0; i < Effects.Count; i++) {

            if (type != Effects[i].Type) continue;
            
            Effects[i] = new EffectInfo(code);
            return;
        }
        Effects.Add(new EffectInfo(code));
    }

    public void SetEffect(EffectInfo info) {
        
        for (int i = 0; i < Effects.Count; i++) {
        
            if (info.Type != Effects[i].Type) continue;
                    
            Effects[i] = info;
            return;
        }

        Effects.Add(info);
    }

    public EffectInfo GetEffect(EffectType type) {
        
        return Effects.FirstOrDefault(effect => effect.Type == type);
    }

    private void SetColor(Color color) {
        hp.ChangeColor(color);
        mp.ChangeColor(color); 
        profile.color = color;
                
    }

    public void SetShield(int code) {
        
        Shield = new(ItemInfo.DefenceType(code)
            ,ItemInfo.DefencePower(code)
            ,ItemInfo.DefenceReflect(code));

    }
    public void SetShield(DefenceType type, float power, bool reflect = false) {

        Shield = new(type, power, reflect);
    }

    public void TurnUpdate() {
        
        Attract.TurnUpdate();
        Shield.TurnUpdate();
        foreach (var effect in Effects) {

            effect.TurnUpdate();
        }

        Effects = Effects.Where(info => info.Type != EffectType.None).ToList();
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

        return skill[index];
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
    
    public bool GetDamage(float damage) {

        damage = Mathf.Ceil(damage);
        bool shieldApply = false;
        (shieldApply, damage) = Shield.ApplyDamage(damage);
        
        CurrentHp -= damage;
        
        if (CurrentHp > 0) {

            ShowDamage(damage, shieldApply);
            return true;
        }

        CurrentHp = 0;
        ShowDamage(damage, shieldApply);

        StartCoroutine(Wait.WaitAndDo(0.6f, () => SetColor(deadGray)));
        
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
   
    private void ShowDamage(float damage, bool shield = false) {

        hp.UpdateInfo(CurrentHp, MaximumHp);
        
        Debug.Log($"Get damage: {damage}");
        hpChange.text = damage.ToString();
        
        if (shield) hpChange.color = shieldColor; 
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
    }

    private void Start() {
        
        hp.SetInfo(CurrentHp, MaximumHp);
        mp.SetInfo(CurrentMp, MaximumMp);
    }
}