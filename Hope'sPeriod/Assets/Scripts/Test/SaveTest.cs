using System;
using Newtonsoft.Json;
using UnityEngine;

public class SaveTest: MonoBehaviour {

    private void Start() { 
        SaveData.Load("DefaultSaveData");
        ScenceControler.Load("PlayMap");
    }
}