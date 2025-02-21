using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow: MonoBehaviour {

    [SerializeField] private GameObject settingWindow;
    [SerializeField] private SettingCategoryButtonManager window;
    private bool on = false;
    private static bool interactable = true;
    
    public static void SetInteractable(bool value) => interactable = value;
    
    private void TurnOn() {
        TilePlayerPhysics.SetMovable(false);
        settingWindow.SetActive(true);
        window.Click();
    }

    private void TurnOff() {

        EverytimeEvent.StartEvent();
        if(ScriptShower.Instance.EventCode == 0)
            TilePlayerPhysics.SetMovable(true);
        settingWindow.SetActive(false);
    }

    private void Start() {
        TurnOff();
    }
    
    private void Update() {

        if (!interactable)
            return;
        
        if (InputManager.Instance.Click(KeyTypes.Cancel)) {
            if (on)
                TurnOff();
            else
                TurnOn();

            on = !on;
        }
    }
}