using UnityEngine;
using System.Collections.Generic;

public class VolumeSetttingWindow: MonoBehaviour {

    [SerializeField] private GameObject window;
    [SerializeField] private MainButtonManager main;
    [SerializeField] private List<VolumeControle> volumeControlers; 
    private bool interactable = false;
    
    public void TurnOn() {
        interactable = true;
        window.SetActive(true);
        foreach (var volumeControler in volumeControlers) {
            volumeControler.ApplyValue();
        }
    }

    public void TurnOff() {
        interactable = false;
        window.SetActive(false);
        main.SetInteractable(true);
    }

    private void Update() {
        if (!interactable)
            return;

        if (InputManager.Instance.Click(KeyTypes.Cancel)) {
            TurnOff();
        }
    }
}