using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectButtonManager: InteractButtonManager {
    
    [SerializeField] private SkillListButtonManger list;
    public override bool Interactable { get; protected set; } = true;
    public override void SelectIn(InteractButton target) {}
    public override void SelectOut(InteractButton target) {}

    public void ShowList(Vector2 pos, int target) {

        list.TurnOn(pos, target);
    }
}