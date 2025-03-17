using System;
using TMPro;
using UnityEngine;

public class TestTextExtension: MonoBehaviour {

    [SerializeField]
    private TMP_Text input;

    private int idx = 0;
    private void Update() {

        idx++;
        if(idx % 5 == 0)
            TextExtension.EffectProcedure(input);
    }
}