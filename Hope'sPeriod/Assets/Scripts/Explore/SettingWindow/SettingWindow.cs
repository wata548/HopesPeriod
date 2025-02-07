using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow: MonoBehaviour {

    [SerializeField] private GameObject settingWindow;
    private bool on = false;

    private void TurnOn() {
        TilePlayerPhysics.SetMovable(false);
        settingWindow.SetActive(true);
    }

    private void TurnOff() {
        
        TilePlayerPhysics.SetMovable(true);
        settingWindow.SetActive(false);
    }

    private void Start() {
        TurnOff();
    }
    
    private void Update() {
        if (InputManager.Instance.Click(KeyTypes.Cancel)) {
            if (on)
                TurnOff();
            else
                TurnOn();

            on = !on;
        }
    }
}