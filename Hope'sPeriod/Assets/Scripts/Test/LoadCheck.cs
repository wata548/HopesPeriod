using TMPro;
using UnityEngine;

public class LoadCheck: MonoBehaviour {

    [SerializeField] private TMP_Text text;

    private void Start() {
        text.text = $"item: {ItemInfo.CheckTable()} + skill: {SkillInfo.CheckTable()}";
    }
}