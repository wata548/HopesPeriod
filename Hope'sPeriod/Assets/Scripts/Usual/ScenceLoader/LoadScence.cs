using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScenceControler {

    public static string targetScence;
    public static void Load(string name) {
        targetScence = name;
        SceneManager.LoadSceneAsync("Loading");
    }

    public static IEnumerator LoadScenceAsync(Action<float> showProgress) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScence);
        operation.allowSceneActivation = false;

        while (!operation.isDone) {

            showProgress?.Invoke(operation.progress);
            if (operation.progress >= 0.9f) {
                showProgress?.Invoke(1);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}