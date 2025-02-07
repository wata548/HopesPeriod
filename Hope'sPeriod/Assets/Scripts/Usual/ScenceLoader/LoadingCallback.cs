using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingCallback: MonoBehaviour {
    private bool updated = false;
    [SerializeField] private Image loading;
    [SerializeField] private TMP_Text loadingProgress;
    [SerializeField] private TMP_Text tipShower;
    [SerializeField] private List<string> tips;
    private void Start() {
        
        int index= Random.Range(0, tips.Count);
        tipShower.text = "Tip: " + tips[index];
        StartCoroutine(ScenceControler.LoadScenceAsync(ProgressUpdate));
    }

    private void ProgressUpdate(float progress) {

        loading.fillAmount = progress;
        loadingProgress.text = $"{(int)(progress * 100)}%";
    }
}