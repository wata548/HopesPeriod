using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class SkillListButtonManager: InteractButtonManager {

    [SerializeField] private SkillInfoShower shower;
    [SerializeField] private SkillSelectButtonManager selectedSkill;
    [SerializeField] private Cursor cursor;
    [SerializeField] private GameObject list;
    [SerializeField] private TMP_Text pageShower;
    
    private EachCharacterInfo info;
    private int index;
    private static readonly Color ActiveColor = Color.red;
    private static readonly Color DisactiveColor = Color.white;

    private int currentPage = 0;
    private int maxPage = 0;
    private const int ShowCount = 5;
    private bool changeable = false;
    
    public override bool Interactable { get; protected set; }
    public override void SelectIn(InteractButton target) {
        var button = Parse(target);

        if (!button.On) return;
        shower.SetSkill(button.Code);
        button.SetColor(ActiveColor);
        cursor.SetIndex(target.Index);
        
    }

    public override void SelectOut(InteractButton target) {
        
        shower.SetSkill(0);
        Parse(target).SetColor(DisactiveColor);
        cursor.Disappear();
    }

    private SkillListButton Parse(InteractButton button) {
        if (button is not SkillListButton result) 
            throw new TypeMissMatched(button.gameObject, typeof(SkillListButton));

        return result;
    }

    public void TurnOn(EachCharacterInfo info, int index) { 

        this.info = info;
        this.index = index;
        Interactable = true;
        list.SetActive(true);
        shower.TurnOn();

        SetMaxPage();
        currentPage = 0;

        SkillListRefresh();
    }

    private void SetMaxPage() {
        int count;
        if (info.GetSkill(index) == 0) {
            changeable = false;
            count = (info.HaveSkills.Count - info.Skill.Count(skill => skill != 0));
        }
        else {
            changeable = true;
            count = info.HaveSkills.Count;
        }
        maxPage = count / ShowCount + (count % ShowCount != 0 ? 1 : 0); 
    }
    
    private void SkillListRefresh() {
        if (maxPage == 0) 
            pageShower.text = $"<0/0>";
        else 
            pageShower.text = $"<{currentPage + 1}/{maxPage}>";
        
        List<int> availableSkill;
        if (!changeable)
            availableSkill = info.HaveSkills.Except(info.Skill).ToList();
        else
            availableSkill = info.HaveSkills.ToList();

        int startPos = currentPage * ShowCount;
        for (int i = 0; i < ShowCount; i++) {
            if (startPos + i >= availableSkill.Count) {
                Parse(buttons[i]).SetCode(0);
                continue;
            }

            Parse(buttons[i]).SetCode(availableSkill[startPos + i]);
        }
    }

    public void NextPage() {
        if (maxPage <= 1)
            return;

        currentPage++;
        if (currentPage >= maxPage)
            currentPage = 0;

        SkillListRefresh();
    }

    public void PriviousPage() {
        if (maxPage <= 1)
            return;

        currentPage--;
        if (currentPage < 0)
            currentPage = maxPage - 1;

        SkillListRefresh();
    }

    public void TurnOff() {
        Interactable = false;
        list.SetActive(false);
        shower.TurnOff();
    }

    public void Click(int code) {
        info.SetSkill(index, code);
        selectedSkill.Refresh();
        SetMaxPage();
        SkillListRefresh();
    }
    
    private void Awake() {
        base.Awake();
        foreach (var button in buttons) {
            Parse(button).SetUp();
        }

        TurnOff();
    }
}