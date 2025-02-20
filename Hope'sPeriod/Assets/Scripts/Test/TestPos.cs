using System;
using TMPro;
using UnityEngine;

public class TestPos: MonoBehaviour {
    private TMP_Text text;
    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void Update() {
        text.text = TilePlayerPhysics.Pos.ToString();
    }
}