using System;
using System.Diagnostics;
using UnityEngine;

public class QuitHandler: MonoBehaviour {
    private void Awake() {
        Application.wantsToQuit += Quit;
    }

    private bool Quit() {
        //Process.GetCurrentProcess().Kill();
        return true;
    }

    public void QuitButton() {
        
        Process.GetCurrentProcess().Kill();
    }
}