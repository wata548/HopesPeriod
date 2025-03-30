using Unity.VisualScripting;
using UnityEngine;

public class M1002Object2: LinearPattern {

    public int HealAmount { get; protected set; } = 8;
    private float damage = 18;
    private float moveSpeed = 20;
    private bool isHeal = false;
    
    private void Disactive(GameObject target) {
        if (target.transform.CompareTag("Player") && !isHeal) {

            isHeal = true;
            MonsterSlider.Instance.Heal(HealAmount);
        }
    }
    
    private void Start() {
        MoveStart = false;
        Damage = damage;
        speed = moveSpeed;
        base.Start();
        ContactStrategy = new TriggerType(Disactive);
    }
}