using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InputExtension {
    
    private static IEnumerable<KeyCode> keyCodes = null;
    
    public static KeyCode GetAnyKeyDown() {

        keyCodes ??= Enum
            .GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Skip(1);
        
        foreach (var keyCode in keyCodes) {
            if (Input.GetKeyDown(keyCode)) {
                return keyCode;
            }
        }

        return KeyCode.None;
    }
}