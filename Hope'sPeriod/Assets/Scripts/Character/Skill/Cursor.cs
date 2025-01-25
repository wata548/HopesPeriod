using UnityEngine;
using System;

[Serializable]
public class PosInfo {
    public Vector2 Pos;
    public float Degree;
}

public class Cursor: MonoBehaviour {
  //==================================================||Field 
     
     private RectTransform rect;
     [SerializeField] private Vector3 interval = new(0, -105);
     [SerializeField] private float defaultDegree = 0;
     [SerializeField] private Vector3 defaultPos = new(-110.4f, 155);
     [SerializeField] private float intervalDegree = 0;
     [SerializeField] private SerializableDictionary<int, PosInfo> SpecialIndexes = new(); 
     
     //==================================================||Property
     
     public int Index { get; private set; } = 0;
 
    //==================================================||Method 
     
     public void TurnOn() {
         SetIndex(0);
     }
 
     public void SetIndex(int index) {

         this.Index = index;
         
         if (SpecialIndexes.TryGetValue(index, out PosInfo info)) {
             rect.localRotation = Quaternion.Euler(0, 0, info.Degree);
             rect.localPosition = info.Pos;
             return;
         }
         
         rect.localRotation = Quaternion.Euler(0, 0, defaultDegree + intervalDegree * index);
         rect.localPosition = defaultPos + interval * index;
     }
     
    //==================================================||Unity Func 
     private void Awake() {
         rect = GetComponent<RectTransform>();
     }       
}