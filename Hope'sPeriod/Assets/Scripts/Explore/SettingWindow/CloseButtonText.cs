using Microsoft.SqlServer.Server;
using TMPro;
using UnityEngine;

public class CloseButtonText: MonoBehaviour {
    [SerializeField] private TMP_Text key;

    public void UpdateText()
        => key.text = InputManager
            .Instance.KeyMapper[KeyTypes.Cancel]
            .key
            .ToString()
            .ToLower();
}