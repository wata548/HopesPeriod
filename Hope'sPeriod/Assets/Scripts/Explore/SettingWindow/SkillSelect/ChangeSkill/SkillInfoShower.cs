using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillInfoShower: MonoBehaviour {

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text tag;
    [SerializeField] private TMP_Text effect;
    [SerializeField] private TMP_Text description;
    [SerializeField] private GameObject @object;

    public void TurnOn() {
        @object.SetActive(true);
    }

    public void TurnOff() {
        @object.SetActive(false);
    }
    
    public void SetSkill(int code) {

        if (code == 0) {
            nameText.text = "";
            tag.text = "";
            effect.text = "";
            description.text = "";
            return;
        }

        nameText.text = SkillInfo.Name(code);
        tag.text = SkillInfo.SimpleTag(code);
        effect.text = "효과: ".AddColor(Color.yellow).SetSize(1.5f) + SkillInfo.Description(code);
        description.text = "설명: ".AddColor(Color.yellow).SetSize(1.5f) + SkillInfo.DetailDescription(code);
    }
}