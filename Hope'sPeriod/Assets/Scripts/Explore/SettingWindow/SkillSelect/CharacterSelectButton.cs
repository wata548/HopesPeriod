using UnityEngine;

public class CharacterSelectButton: InteractButtonUI {

    [SerializeField] private ShowSkillTemp skillShower;
    [SerializeField] private GameObject button;
    private EachCharacterInfo info;

    public void SetInfo(EachCharacterInfo info) => this.info = info;

    public void TurnOn() {
        if (!info.Exist) {
            button.SetActive(false);
            return;
        }
        
        button.SetActive(true);
        skillShower.SetSkill(info.Skill);
    }
    public override void Click() {
        
    }
}