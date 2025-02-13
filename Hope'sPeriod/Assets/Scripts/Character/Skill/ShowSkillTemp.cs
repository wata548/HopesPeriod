using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ShowSkillTemp: MonoBehaviour {
    [SerializeField] private TMP_Text[] skills;

    public void SetSkill(List<int> skills) {

        int index = 0;
        foreach (var skill in this.skills) {
            if (skills[index] == 0)
                skill.text = $"     빈 슬롯".AddColor(Color.gray);
            
            else 
                skill.text = $" {index+1}. {SkillInfo.Name(skills[index]).SetSize(0.8f)}";
            index++;
        }
    }
}