using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public enum PowerUpType {

     PERCENT,
     POWER
};

[CreateAssetMenu(menuName = "Skill/PowerUp")]
public class PowerUpSkill : ScriptableObject, ISkill {
     
     [SerializeField] private string koreanName;
     [SerializeField] private string info; 
     public string KoreanName => koreanName;
     public string Info => info;
     
     [SerializeField] float requireHP; 
     [SerializeField] float requireMP; 
     public float RequireHP => requireHP;
     public float RequireMP => requireMP;
     
     [SerializeField] private float power;
     [SerializeField] private int duration; 
     [SerializeField] private AccelerateType accelerateType; 
     [SerializeField] private float accelerate; 
     public float Power => power;
     public int Duration => duration;
     public AccelerateType AccelerateType => accelerateType;
     public float Accelerate => accelerate;
     
     [SerializeField] private PowerUpType powerUpType;
     [SerializeField] private TargetType target;
     public TargetType TargetType => target;
     public PowerUpType PowerUpType => powerUpType;
}
