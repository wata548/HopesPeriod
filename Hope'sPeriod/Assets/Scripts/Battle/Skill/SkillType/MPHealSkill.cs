using UnityEngine;

[CreateAssetMenu(fileName = "MPHealSkill", menuName = "Scriptable Objects/MPHealSkill")]
public class MPHealSkill : ScriptableObject, ISkill {

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
     [SerializeField] private float accelerate; 
     public float Power => power;
     public int Duration => duration;
     public float Accelerate => accelerate;

     [SerializeField] private TargetType target;
     public TargetType TargetType => target;
}