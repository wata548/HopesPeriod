using System;
using UnityEngine;

public class ItemListCursor: MonoBehaviour {
    
   //==================================================||Field 
    
    private RectTransform rect;
    private readonly Vector3 interval = new(0, -105);
    private readonly Vector3 defaultPos = new(-110.4f, 155);
    
    //==================================================||Property
    
    public int Index { get; private set; } = 0;
    public static ItemListCursor Instance { get; private set; } = null;

   //==================================================||Method 
    
    public void TurnOn() {
        SetIndex(0);
    }

    public void SetIndex(int index) {
        
        this.Index = index;
        rect.localPosition = defaultPos + interval * index;
    }
    
   //==================================================||Unity Func 
    private void Awake() {
        rect = GetComponent<RectTransform>();
            
        if (Instance == null) 
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }
}