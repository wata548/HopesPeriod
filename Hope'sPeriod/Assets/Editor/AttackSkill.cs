using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Skill/Attack")]
public class AttackSkill : ScriptableObject, ISkill {

     //Before version need this function but current don't need it
     /*[MenuItem("Assets/Create/Skills/AttackSkill")]
     public static void MakeInstance() {
          
          var newInstance = ScriptableObject.CreateInstance<AttackSkill>();
          
          AssetDatabase.CreateAsset(newInstance, "Assets/Resources/Skills/NewSkill.asset");
          AssetDatabase.Refresh();
     }*/
     
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
     public float Accelerate => accelerate;
     public AccelerateType AccelerateType => accelerateType;
}

