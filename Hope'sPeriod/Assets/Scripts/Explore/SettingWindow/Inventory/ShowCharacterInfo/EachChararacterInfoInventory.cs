using System;
using UnityEngine;

public class EachChararacterInfoInventory: MonoBehaviour {

    [SerializeField] private string characterName;
    [SerializeField] private InventoryCharacterSlider hpBar;
    [SerializeField] private InventoryCharacterSlider mpBar;
    [SerializeField] private GameObject image;
    private EachCharacterInfo info;
    public EachCharacterInfo Info => info;

    
    private void TurnOn() {
        info = GameObject.Find($"{characterName}Info").GetComponent<EachCharacterInfo>();
        hpBar.SetState(info.CurrentHp, info.MaximumHp);
        mpBar.SetState(info.CurrentMp, info.MaximumMp);
        ShowUpdate();
    }

    private void SetView(bool view) {
        hpBar.gameObject.SetActive(view);
        mpBar.gameObject.SetActive(view);
        image.SetActive(view);
    }
    
    public void ShowUpdate() {
         if (!info.Exist) SetView(false);
         else SetView(true);
    }
    
    public void UpdateState() {

        hpBar.UpdateState(info.CurrentHp, info.MaximumHp);
        mpBar.UpdateState(info.CurrentMp, info.MaximumMp);
        ShowUpdate();
    }
    private void Awake() {
        TurnOn();
    }
}