using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverButton: MonoBehaviour {
    [SerializeField] private TMP_Text load;
    [SerializeField] private LoadButtonManager loadManager;
    private void Awake() {
        if (CurrentMonsterInfo.IsBoss) {
            load.text = "재도전하기";
        }
        else
            load.text = "불러오기";
    }

    public void LoadButton() {
        
        if (!CurrentMonsterInfo.IsBoss) {
            loadManager.TurnOn();
        }
        else {
            SaveData.Load(-1);
            CurrentMonsterInfo.MakeMonster();
            ScenceControler.Load("Battle");
        }
    }

    public void Title() {
        
        ScenceControler.Load("Title");
    }
}