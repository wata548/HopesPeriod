using System;
using UnityEngine;

public class EachChararacterInfoInventory: MonoBehaviour {

    [SerializeField] private string characterName;
    [SerializeField] private InventoryCharacterSlider hpBar;
    [SerializeField] private InventoryCharacterSlider mpBar;
    private EachCharacterInfo info;
    public EachCharacterInfo Info => info;

    private void TurnOn() {
        info = GameObject.Find($"{characterName}Info").GetComponent<EachCharacterInfo>();
        hpBar.SetState(info.CurrentHp, info.MaximumHp);
        mpBar.SetState(info.CurrentMp, info.MaximumMp);
    }
    
    public void UpdateState() {

        hpBar.UpdateState(info.CurrentHp, info.MaximumHp);
        mpBar.UpdateState(info.CurrentMp, info.MaximumMp);
    }
    private void Start() {
        TurnOn();
    }
}