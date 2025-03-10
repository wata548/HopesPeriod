using TMPro;
using UnityEngine;

public enum VolumeType {
    Master,
    UI,
    Enviroment
}

public class VolumeControle: MonoBehaviour {
    [SerializeField] private TMP_Text valueShower;
    [SerializeField] private VolumeType type;
    [SerializeField] private UnityEngine.UI.Slider slider;

    public void ApplyValue() {

        var value = type switch {
            VolumeType.Master => AudioManager.Instance.MasterVolume,
            VolumeType.UI => AudioManager.Instance.UIVolume,
            VolumeType.Enviroment => AudioManager.Instance.EnvirmentVolume

        };
        
        valueShower.text = $"{(int)(value * 100)}%";
        slider.value = value;
    }
    
    public void ChangeValue() {

        var value = slider.value;
        switch (type) {
            case VolumeType.Master:
                AudioManager.Instance.MasterVolume = value;
                break;
            case VolumeType.Enviroment:
                AudioManager.Instance.EnvirmentVolume = value;
                break;
            case VolumeType.UI:
                AudioManager.Instance.UIVolume = value;
                break;
        }

        valueShower.text = $"{(int)(value * 100)}%";
        AudioManager.Instance.RefreshVolume();
    }
}