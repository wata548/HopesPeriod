using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using SpreadInfo;
using TMPro;

public class EachCharacterInfoBattle : MonoBehaviour {
    //==================================================||Set Inspector 

    [SerializeField] private string targetName;
    [SerializeField] private SpriteRenderer profile;
    [SerializeField] private CharacterSlider hp;
    [SerializeField] private CharacterSlider mp;
    [SerializeField] private TMP_Text hpChange;
    [SerializeField] private TMP_Text mpChange;
    private EachCharacterInfo info;
    private List<ShowEffect> showEffect = new(); 
    
    //==================================================||Field 
    
    private readonly Color healHpColor = Color.green;
    private readonly Color healMpColor = Color.green;
    private readonly Color shieldColor = Color.cyan;
    private readonly Color damageHpColor = Color.white;
    private readonly Color useMpColor = Color.white;
    private readonly Color deadGray = new(0.3f, 0.3f, 0.3f);
    
    //==================================================| Properties
    public List<EffectInfo> Effects { get; private set; } = new();
    public ShieldInfo Shield { get; private set; } = new (DefenceType.None, 0, false);
    public AttractInfo Attract { get; private set; } = new();
    public bool Dead => info.Dead;
    public bool Exist => info.Exist;
    public float MaximumHp => info.MaximumHp;
    public float CurrentHp => info.CurrentHp;
    public float MaximumMp => info.MaximumMp;
    public float CurrentMp => info.CurrentMp;
    //public string Name => Regex.Match(info.gameObject.name, @"(.*)Info").Groups[1].Value;
    private Color originColor;

    public void SetEffectImage() {

        int index = 0;
        if (Shield.Type != DefenceType.None) {
            
            if (showEffect.Count >= index)
                Generate(index);
            showEffect[index++].Set(Shield);
        }
        if (Attract.Duration > 0) {
            if (showEffect.Count >= index)
                Generate(index);
            showEffect[index++].Set(Attract);
        }

        foreach (var effect in Effects) {
            if (showEffect.Count >= index)
                Generate(index);
            showEffect[index++].Set(Attract);
        }

        for (int i = index; i < showEffect.Count; i++) {
            showEffect[i].Destroy();
        }

        showEffect = showEffect.Take(index).ToList();


        ShowEffect Generate(int index) {

            const float size = 0.6f;

            var image = Instantiate(Resources.Load<GameObject>("EffectImage/Icon"), transform);
            
            var effect = image
                .GetComponent<ShowEffect>();
            showEffect.Add(effect);

            var interval = Vector3.zero;
            if (index % 2 == 1) {
                interval.y = -size;
            }

            interval.x = (index / 2) * size;
            image.transform.localPosition += interval;
            
            return effect;
        }
    }
    
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
    public bool UseMp(float power) {
        if (info.UseMp(power)) {
            
            ShowUseMp(power);
            return true;
        }

        return false;
    }
    public bool HealMp(float power) {

        if (info.HealMp(power)) {
            
            ShowHealMp(power);
            return true;
        }

        return false;
    }
    public bool GetSafeDamage(float damage) {

        if (info.GetSafeDamage(damage)) {
            
            ShowDamage(damage);
            return true;
        }

        return false;
    }
        
    public bool GetDamage(float damage) {
    
        damage = Mathf.Ceil(damage);
        bool shieldApply = false;
        (shieldApply, damage) = Shield.ApplyDamage(damage);

        if (info.GetDamage(damage)) {
            
            ShowDamage(damage, shieldApply);
            return true;
        }
        else {
            
            ShowDamage(damage, shieldApply);
            DeadProcess();
            return false;
        }
    }

    private void DeadProcess() {
        StartCoroutine(Wait.WaitAndDo(0.6f, () => SetColor(deadGray)));
    }
    
    public bool HealHp(float power, bool revive = false) {

        bool dead = info.Dead;

        if (info.HealHp(power, revive)) {
            if(dead && !info.Dead)    
                StartCoroutine(Wait.WaitAndDo(0.6f, () => SetColor(originColor)));

            ShowHealHp(power);
            return true;
        }    
            
        return false;
    }
       
    private void ShowDamage(float damage, bool shield = false) {
    
        hp.UpdateInfo(info.CurrentHp, info.MaximumHp);
            
        Debug.Log($"Get damage: {damage}");
        hpChange.text = damage.ToString();
            
        if (shield) hpChange.color = shieldColor; 
        else hpChange.color = damageHpColor;
            
        hpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
        
    private void ShowHealHp(float power) {
            
        hp.UpdateInfo(info.CurrentHp, info.MaximumHp);
            
        Debug.Log($"Heal hp: {power}");
        hpChange.text = power.ToString();
        hpChange.color = healHpColor;
            
        hpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
    private void ShowUseMp(float power) {
        
        mp.UpdateInfo(info.CurrentMp, info.MaximumMp);
            
        Debug.Log($"Use Mp: {power}");
        mpChange.text = power.ToString();
        mpChange.color = useMpColor;
            
        mpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }
    private void ShowHealMp(float power) {
    
        mp.UpdateInfo(info.CurrentMp, info.MaximumMp);
            
        Debug.Log($"Heal mp: {power}");
        mpChange.text = power.ToString();
        mpChange.color = healMpColor;
            
        mpChange.DOBlink(0.2f, 0.2f, 0.2f, 0.7f);
    }

    public void SetSkill(int index, int code) => info.SetSkill(index, code);
    public int GetSkill(int index) => info.GetSkill(index);
    
    
    private void Awake() {
    
        originColor = profile.color;
        info = GameObject.Find($"{targetName}Info")
            .GetComponent<EachCharacterInfo>();

        if (!info.Exist) gameObject.SetActive(false);
    }

    private void Start() {

        if (info.Dead) DeadProcess();
        hp.SetInfo(info.CurrentHp, info.MaximumHp);
        mp.SetInfo(info.CurrentMp, info.MaximumMp);
    }
}