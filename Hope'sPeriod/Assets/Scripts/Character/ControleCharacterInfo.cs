using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ControleCharacterInfo: MonoBehaviour {
    
    
    //==================================================| Field 
    public static ControleCharacterInfo Instance { get; private set; } = null;
    
    public int CharacterCount { get; private set; } = 3;
    private const int MaximumCharacterCount = 3;
    [field: SerializeField] private ControleEachCharacterInfo[] characterInfos = new ControleEachCharacterInfo[3];
    private const float InvincibilityTime = 1.2f;
    
    //==================================================| Method 
    #region Singleton 
    void SetSingleton() {
        
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(this);
    }
    
    #endregion

    public ControleEachCharacterInfo CharacterInfo(int index) => characterInfos[index];
    public ControleEachCharacterInfo[] CharacterInfos() => characterInfos;
    
    public bool Alive(int index) {
        return !characterInfos[index].Dead;
    }
    
    public bool DamageDistribute(float power) {

        ShakeCamera.Instance.Shake(0.2f, 0.1f);
        
        // find who use attract skill 
        var useAttrackSkillplayer =
            characterInfos
                .Take(CharacterCount)
                .OrderByDescending(character => character.Attract)
                .First();

        // check no one used skill
        bool skillUse = !Mathf.Approximately(useAttrackSkillplayer.Attract, 0);
        if (skillUse) {

            // attract skill is always ratio, must (ratio <= 1).
            if (useAttrackSkillplayer.Attract > 1) {

                throw new OutOfRange(0, 1, useAttrackSkillplayer.Attract);
            }
            float damage = power * useAttrackSkillplayer.Attract;
            damage = Mathf.Ceil(damage);
            
            useAttrackSkillplayer.GetDamage(damage);

            power -= damage;
        }

        else {
            
            // who used skill is not exist
            useAttrackSkillplayer = null;
        }

        // make array didn't contain who used skill
        var checkCharacters = characterInfos
            .Where(factor => factor != useAttrackSkillplayer && !factor.Dead)
            .ToArray();
        
        // sum attract
        float sumHP = checkCharacters.Sum(factor => factor.MaximumHp);
        
        foreach (var checkCharacter in checkCharacters) {

            float eachPower = checkCharacter.MaximumHp / sumHP * power;
            eachPower = Mathf.Ceil(eachPower);
            checkCharacter.GetDamage(eachPower);
        }
        
        return IsGameover();
    }
    public bool IsGameover() {

        bool gameover = characterInfos.Any(character => character.Dead == false);
        return gameover;
    }
    
    //=================================================| UnityLogic 
    private void Awake() {
    
        SetSingleton();
    }
}