using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestSelectWindow: MonoBehaviour {

    private void Update() {

        if (Input.GetKeyDown(KeyCode.W)) {
            ScriptShower.Instance.StartScript(6999);
        }
    }
}