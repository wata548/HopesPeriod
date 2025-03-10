using System.Collections.Generic;
using UnityEngine;

public class SettingWindowManager: MonoBehaviour {

    [SerializeField] private GameObject window;
    [SerializeField] private KeyBind keySetting;
    [SerializeField] private List<VolumeControle> volumeControlers;
    
    public void TurnOn() {
        
        window.SetActive(true);
        keySetting.Refresh();
        foreach (var volumeConter in volumeControlers) {
            volumeConter.ApplyValue();
        }
    }

    public void TurnOff() {
        window.SetActive(false);
        keySetting.ShowerTurnOff();
    }
}