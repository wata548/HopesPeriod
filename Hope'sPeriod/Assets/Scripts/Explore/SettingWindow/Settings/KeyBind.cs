using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class KeyBind: MonoBehaviour {

    [SerializeField] private TMP_Text shower;
    private static int select = 0; 
    
    private readonly static List<(string korean, string fileName)> bindType = new () {
        ("WASD", "Default"),
        ("화살표", "DefaultArrow"),
    };

    private readonly static Dictionary<KeyTypes, string> keyTypeKorean = new() {
        { KeyTypes.Cancel, "취소" },
        { KeyTypes.Select, "선택 / 상호 작용" },
        { KeyTypes.Up, "위쪽" },
        { KeyTypes.Down, "아래쪽" },
        { KeyTypes.Left, "왼쪽" },
        { KeyTypes.Right, "오른쪽" },
    };

    private int index = 0;

    public void Show() {
        shower.text = bindType[select].korean;
    }

    public void SelectKeySetting() {
        InputManager.Instance.KeySettingLoad(bindType[select].fileName);
    }

    public string ShowSettng(string fileName) {
        var keys = Enum
            .GetValues(typeof(KeyTypes))
            .Cast<KeyTypes>()
            .Where(key => key != KeyTypes.Interaction)
            .ToList();

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

        return info;
    }
    
    public void NextButton() {
        index = (index + 1) % 2;
    }
}