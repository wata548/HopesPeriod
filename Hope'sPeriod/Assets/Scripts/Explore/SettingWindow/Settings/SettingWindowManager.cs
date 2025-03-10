using UnityEngine;

public class SettingWindowManager: MonoBehaviour {

    [SerializeField] private GameObject window;
    [SerializeField] private KeyBind keySetting;
    
    public void TurnOn() {
        
        window.SetActive(true);
        keySetting.Refresh();
    }

    public void TurnOff() {
        window.SetActive(false);
        keySetting.ShowerTurnOff();
    }
}