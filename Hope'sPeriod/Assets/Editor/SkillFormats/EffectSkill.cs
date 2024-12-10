using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;
[CreateAssetMenu(menuName = "Skill/Effect")]
public class EffectSkill : ScriptableObject, ISkill {

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
     public AccelerateType AccelerateType=> accelerateType;
     public float Accelerate => accelerate;
     
     [SerializeField] private EffectType effectType;
     [SerializeField] private TargetType target;
     public TargetType TargetType => target;
     public EffectType EffectType => effectType;
}