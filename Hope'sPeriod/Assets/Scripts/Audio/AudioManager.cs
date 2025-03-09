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
            Destroy(gameObject);
        }
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