using System;
using UnityEngine;

public class M1002Object4: BaseEnemy {
    public override CompositeGroupBase MoveComposite { get; protected set; }
    public override ContactStrategy ContactStrategy { get; protected set; }
    public override float Damage { get; protected set; } = 20;
    public int HealAmount { get; protected set; } = 9;
    private bool isHeal = false;
    
    private void Disactive(GameObject target) {
        if (target.transform.CompareTag("Player") && !isHeal) {
            isHeal = true;
            MonsterSlider.Instance.Heal(HealAmount);
        }
     }

    private void Start() {
        ContactStrategy = new TriggerType(Disactive);
    }

    public override void Move() {
    }
}
