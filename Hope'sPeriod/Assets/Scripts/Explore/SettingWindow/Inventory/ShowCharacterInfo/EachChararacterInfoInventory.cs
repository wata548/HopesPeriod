using System;
using UnityEngine;

public class EachChararacterInfoInventory: MonoBehaviour {

    [SerializeField] private string characterName;
    [SerializeField] private InventoryCharacterSlider hpBar;
    [SerializeField] private InventoryCharacterSlider mpBar;
    [SerializeField] private GameObject image;
    private EachCharacterInfo info;
    public EachCharacterInfo Info => info;
    
    public void TurnOn() {
        
        hpBar.SetState(info.CurrentHp, info.MaximumHp, info.Dead);
        mpBar.SetState(info.CurrentMp, info.MaximumMp, info.Dead);
        ShowUpdate();
    }

    private void SetView(bool view) {
        hpBar.gameObject.SetActive(view);
        mpBar.gameObject.SetActive(view);
        image.SetActive(view);
    }

    private void ShowUpdate() {
         if (!info.Exist) SetView(false);
         else SetView(true);
    }
    
    public void UpdateState() {

        hpBar.UpdateState(info.CurrentHp, info.MaximumHp, info.Dead);
        mpBar.UpdateState(info.CurrentMp, info.MaximumMp, info.Dead);
        ShowUpdate();
    }
    private void Awake() {
        
        info = GameObject.Find($"{characterName}Info").GetComponent<EachCharacterInfo>();
    }
}