using System;
using System.Linq;
using SpreadInfo;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class ControleCharacterInfo: MonoBehaviour {
    
    //==================================================||Set Inspector 
   
    [field: SerializeField] private ControleEachCharacterInfo[] characterInfos = new ControleEachCharacterInfo[3];
    //==================================================| Field 
    public bool GameOver { get; private set; } = false;
    
    private const int MaximumCharacterCount = 3;
    private const float InvincibilityTime = 1.2f;
    
    //==================================================||Property 
    public static ControleCharacterInfo Instance { get; private set; } = null;
    public int CharacterCount { get; private set; } = 3;

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

    public int GetSkill(int characterIndex, int skillIndex) {
        
        return characterInfos[characterIndex].GetSkill(skillIndex);
    }
    
    public ControleEachCharacterInfo CharacterInfo(int index) => characterInfos[index];
    public ControleEachCharacterInfo[] CharacterInfos() => characterInfos;

    public void TurnUpdate() {
        foreach (var character in characterInfos) {

            character.TurnUpdate();
        }
    }
    
    public void ShieldOn(DefenceType type = DefenceType.Time, float power = 2.3f) {

        foreach (var character in characterInfos) {

            character.SetShield(type, power);
        }
        
    }

    public void ShieldOff() {

        foreach (var character in characterInfos) {
            
            character.SetShield(DefenceType.None, 0);
        }
    }

    public void ShieldOff(int index) {

        characterInfos[index].SetShield(DefenceType.None, 0);
    }
    
    public bool Alive(int index) {
        return !characterInfos[index].Dead;
    }
    
    public bool DamageDistribute(float power) {

        ShakeCamera.Instance.Shake(0.2f, 0.1f);
        
        // find who use attract skill 
        var useAttrackSkillplayer =
            characterInfos
                .Take(CharacterCount)
                .Where(character => !character.Dead)
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
        
        return IsGameOver();
    }
    private bool IsGameOver() {

        bool gameOver = characterInfos.All(character => character.Dead);
        if (gameOver) {
            StartCoroutine(Wait.WaitAndDo(1, () => SceneManager.LoadScene("GameOver")));
        }
        return GameOver = gameOver;
    }
    
    //=================================================| UnityLogic 
    private void Awake() {
    
        SetSingleton();
    }
}