using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.Serialization;

public class KeyBind: MonoBehaviour {

    
   //==================================================||Serialize 
    [SerializeField] private TMP_Text settingShower;
    [SerializeField] private GameObject showKey;
    [SerializeField] private TMP_Text showKeyText;
    [SerializeField] private CloseButtonText closeButton;
   //==================================================||Members 
    private static int select = 0; 
    private static int index = 0;
    private static readonly Color Selected = Color.red;
    private static readonly Color NotSelected = Color.gray;
    private bool isShowerOn = false;
    
    private readonly static List<(string korean, string fileName)> bindType = new () {
        ("WASD", "Default"),
        ("화살표", "DefaultArrow"),
    };

    private readonly static Dictionary<KeyTypes, string> keyTypeKorean = new() {
        { KeyTypes.Cancel, "취소" },
        { KeyTypes.Select, "선택 / 상호 작용".SetSize(0.7f) },
        { KeyTypes.Up, "위쪽" },
        { KeyTypes.Down, "아래쪽" },
        { KeyTypes.Left, "왼쪽" },
        { KeyTypes.Right, "오른쪽" },
    }; 
    //==================================================||Method
    public void SelectKeySetting() {
        select = index;
        InputManager.Instance.KeySettingLoad(bindType[select].fileName);
        Refresh();
        closeButton.UpdateText();
    }

    public void ShowSetting() {
        var keys = Enum
            .GetValues(typeof(KeyTypes))
            .Cast<KeyTypes>()
            .Where(key => key != KeyTypes.Interaction && key != KeyTypes.Jump)
            .ToList();

        var fileName = bindType[index].fileName;
        var Mapinginfo = InputManager.Instance.DeserializeJson(fileName);
        string info = "";
        
        foreach (var key in keys) {

            var targetKey = Mapinginfo[key].key;
            var keyName = targetKey switch {
                KeyCode.UpArrow => "↑".SetSize(1.5f),
                KeyCode.DownArrow => "↓".SetSize(1.5f),
                KeyCode.LeftArrow => "→".SetSize(1.5f),
                KeyCode.RightArrow => "←".SetSize(1.5f),
                KeyCode.Return => "Enter",
                KeyCode.Escape => "Esc",
                _ => targetKey.ToString()
            };

            info += $"{keyTypeKorean[key]} : {keyName}\n";
        }

        isShowerOn = true;
        showKey.SetActive(true);
        showKeyText.text = info;
    }
    
    public void NextButton() {
        index = (index + 1) % 2;
        Refresh();
    }

    public void Refresh() {
        
        settingShower.text = bindType[index].korean;
        settingShower.color = index == select ? Selected : NotSelected;

        if (isShowerOn) {
            ShowSetting();
        }
    }

    public void ShowerTurnOff() {
        showKey.SetActive(false);
        isShowerOn = false;
    }
    
    private void Update() {

        if (isShowerOn && InputManager.Instance.Click(KeyTypes.Cancel)) {
            ShowerTurnOff();
        }
    }
}