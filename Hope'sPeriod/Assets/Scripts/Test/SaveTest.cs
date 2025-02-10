using System;
using Newtonsoft.Json;
using UnityEngine;

public class SaveTest: MonoBehaviour {

    [SerializeField] private SaveFormat a;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            a = SaveData.Load("DefaultSaveData");
            Debug.Log(a.SavePos.Code);

        }
    }
}