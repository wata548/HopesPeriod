using System;
using UnityEngine;

public class ItemListCursor: MonoBehaviour {
    private int index = 0;
    private RectTransform rect;
    private readonly Vector3 interval = new(0, -105);
    private readonly Vector3 defaultPos = new(-110.4f, 155);
    public static ItemListCursor Instance { get; private set; } = null;
    
        
    private void Awake() {
        rect = GetComponent<RectTransform>();
        
        if (Instance == null) 
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public void SetIndex(int index) {
        this.index = index;
        rect.localPosition = defaultPos + interval * index;
    }
}