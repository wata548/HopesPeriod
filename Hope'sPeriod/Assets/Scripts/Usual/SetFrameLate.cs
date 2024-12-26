using System;
using UnityEngine;

public class SetFrameLate : MonoBehaviour {
    public static SetFrameLate Instance { get; private set; } = null;

    private int frame = 60;

    public void Set() => Application.targetFrameRate = frame;

    public void Set(int frame) {

        this.frame = frame;
        Set();
    }
    private void Awake() {

        Set();
    }
}
