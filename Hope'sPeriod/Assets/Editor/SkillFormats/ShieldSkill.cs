using UnityEngine;
using System.Collections;
using UnityEditor;

public enum ShieldType {
     CRASH,
     DURACTION,
     HIT_COUNT
}

[CreateAssetMenu(menuName = "Skill/Shield")]
public class ShieldSkill : ScriptableObject, ISkill {
    
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
     [SerializeField] private AccelerateType acelateType;
     [SerializeField] private float accelerate; 
     public float Power => power;
     public int Duration => duration;
     public AccelerateType AccelerateType => acelateType;
     public float Accelerate => accelerate;

     [SerializeField] private TargetType target;
     [SerializeField] private bool reflect;
     [SerializeField] private ShieldType shieldType;
     public TargetType TargetType => target;
     public bool Reflect => reflect;
     public ShieldType ShieldType => shieldType;
}
