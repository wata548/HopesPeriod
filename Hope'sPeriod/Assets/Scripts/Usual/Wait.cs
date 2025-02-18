using System.Collections;
using UnityEngine;
using System;

public class Wait {
    public static IEnumerator WaitAndDo(float time, Action action) {
        yield return new WaitForSeconds(time);
        action();
    }
    
    public static IEnumerator WaitAndDo(Func<bool> condition, Action action) {
        yield return new WaitUntil(condition);
        action();
    }
}