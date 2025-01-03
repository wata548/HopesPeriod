using System;
using System.Linq;
using UnityEngine;

public class ControleCharacterInfo: MonoBehaviour {
    
    
    //==================================================| Field 
    public static ControleCharacterInfo Instance { get; private set; } = null;
    
    private int characterCount = 0;
    private const int MaximumCharacterCount = 3;
    private readonly ControleEachCharacterInfo[] characterInfos = new ControleEachCharacterInfo[3];
    private const float InvincibilityTime = 1.2f;
    
    #region Singleton 
    void SetSingleton() {
        
        if (Instance == null) {
            Instance = this;
        }
    }
    
    #endregion
    //==================================================| Method 
    public bool DamageDistribute(float power) {

        // find who use attract skill 
        var useAttrackSkillplayer = 
            characterInfos
                .Take(characterCount)
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
            .Where(factor => factor != useAttrackSkillplayer)
            .ToArray();
        
        // sum attract
        float sumAttract = checkCharacters.Sum(factor => factor.Attract);
        
        foreach (var checkCharacter in checkCharacters) {

            float eachPower = checkCharacter.Attract / sumAttract * power;
            eachPower = Mathf.Ceil(eachPower);
            checkCharacter.GetDamage(eachPower);
        }
        
        bool gameover = characterInfos.Any(character => character.Dead == false);
        return gameover;
    }
    public bool AddCharacter(ControleEachCharacterInfo character) {
        
        if (characterCount >= MaximumCharacterCount) {

            return false;
        }

        characterInfos[characterCount++] = character;
        return true;
    }

    public bool ChangePosition(int index1, int index2) {
            
        if (Mathf.Max(index1, index2) >= characterCount) {
            
            return false;
        }

        (characterInfos[index1], characterInfos[index2]) = (characterInfos[index2], characterInfos[index1]);
        return true;
    }
    
    //=================================================| UnityLogic 
    private void Awake() {
    
        SetSingleton();
    }
}