using System;
using UnityEngine;

public class WaveMove: MonoBehaviour {

    [SerializeField]
    private float amplification;

    [SerializeField] 
    private float cycle;

    [SerializeField] 
    private bool horizontal = false;
    
    private float startTime;
    private Vector3 startPos;
    
    public void SetUp() {
        startTime = Time.time;
        startPos = transform.localPosition;
    }

    private void Update() {

        float progress = Time.time - startTime;
        float delta = amplification * Mathf.Cos(progress * Mathf.PI * cycle);
        var pos = startPos;
        if (horizontal)
            pos.x += delta;
        else {
            pos.y += delta;
        }

        transform.localPosition = pos;
    }
}