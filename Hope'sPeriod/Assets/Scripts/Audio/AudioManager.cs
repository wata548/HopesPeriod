using System;
using System.Net.WebSockets;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager: MonoBehaviour {

    public static AudioManager Instance { get; private set; } = null;

    private float masterVolume = 1;
    public float MasterVolume {
        get => masterVolume;
        set => masterVolume = Mathf.Clamp(0, value, 1);
    }
    private Bus masterBus;
    
    private float uIVolume = 1;
    public float UIVolume {
        get => uIVolume;
        set => uIVolume = Mathf.Clamp(0, value, 1);
    }
    private Bus UIBus;
    
    private float enviromentVolume = 1;
    public float EnvirmentVolume {
        get => enviromentVolume;
        set => enviromentVolume = Mathf.Clamp(0, value, 1);
    }
    private Bus EnviromentBus;
    
    private void Awake() {
        
        if (Instance is null) {
            
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        masterBus = RuntimeManager.GetBus("bus:/");
        UIBus = RuntimeManager.GetBus("bus:/UI");
        EnviromentBus = RuntimeManager.GetBus("bus:/Enviroment");
    }

    public void RefreshVolume() {
        masterBus.setVolume(MasterVolume);
        UIBus.setVolume(UIVolume);
        EnviromentBus.setVolume(EnvirmentVolume);
    }

    public bool IsPlaying(EventInstance sound) {
        PLAYBACK_STATE state;
        sound.getPlaybackState(out state);
        return state == PLAYBACK_STATE.PLAYING;
    }
    
    public EventInstance CreateInstance(EventReference sound)
        => RuntimeManager.CreateInstance(sound);

    public void PlayOne(EventReference sound)
        => RuntimeManager.PlayOneShot(sound);
}