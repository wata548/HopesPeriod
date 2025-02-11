using System;
using TMPro;
using UnityEngine;

public class SkillListButtonManger: InteractButtonManager {

    
    public override bool Interactable { get; protected set; } = true;
    public override void SelectIn(InteractButton target) { }
    public override void SelectOut(InteractButton target) {}

    private const int ShowCount = 4;
    private int index = 0;
    private int page = 0;
    private int maxPage = 0;
    private EachCharacterInfo target;
    [SerializeField] private TMP_Text pageText;
    [SerializeField] private GameObject list;
    
    private RectTransform rect;
    
    public void TurnOn(Vector2 pos, int index) {

        rect.localPosition = pos;
        this.index = index;
        Interactable = true;
        list.SetActive(true);
        page = 0;
        target = CharacterInfoInventory.Instance.Characterinfos[index].Info;
        maxPage = target.HaveSkills.Count / ShowCount + (target.HaveSkills.Count % ShowCount != 0 ? 1 : 0);

        var skills = target.HaveSkills;
        for (int i = 0; i < ShowCount; i++) {

            int code = 0;
            if (skills.Count > i) {

                code = skills[i];
            }

            Parse(buttons[i]).SetCode(code);
        }
    }

    private SkillListButton Parse(InteractButton button) {
        if (button is not SkillListButton result) {
            throw new TypeMissMatched(button.gameObject, typeof(SkillListButton));
        }

        return result;
    }

    private void Awake() {
        base.Awake();
        rect = list.GetComponent<RectTransform>();
    }

}