using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class FmodEvents: MonoBehaviour {

    public static FmodEvents Instance { get; private set; } = null;
    [Header("UI")] 
    [field: SerializeField] public EventReference SelectIn { get; private set; }
    [field: SerializeField] public EventReference Click { get; private set; }

    [Header("Enviroment")]
    [field: SerializeField] public EventReference Walk { get; private set; }
    
    private void Awake() {
        if (Instance is null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
}