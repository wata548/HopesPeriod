using UnityEngine;

public class CharacterSelectButton: InteractButtonUI {

    [SerializeField] private ShowSkillTemp skillShower;
    private EachCharacterInfo info;

    public void SetInfo(EachCharacterInfo info) => this.info = info;

    public void TurnOn() {
        skillShower.SetSkill(info.Skill);
    }
    public override void Click() {
        
    }
}