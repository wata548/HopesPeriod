using System;
using TMPro;
using UnityEngine;

public class ShowState : MonoBehaviour {
    
    private TMP_Text text;
    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    void Update() {
        text.text = $"{GameFSM.Instance.State.ToString()} + {GameFSM.Instance.PlayerTurnState.ToString()}";
    }
}
