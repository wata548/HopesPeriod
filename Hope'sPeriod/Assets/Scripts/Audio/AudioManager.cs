using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager: MonoBehaviour {

    public static AudioManager Instance { get; private set; } = null;

    private void Awake() {
        
        if (Instance is null) {
            
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public void PlayOne(EventReference sound)
        => RuntimeManager.PlayOneShot(sound);
}