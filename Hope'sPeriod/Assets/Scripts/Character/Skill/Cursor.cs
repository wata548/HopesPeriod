using UnityEngine;
using System;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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
     [SerializeField] private SerializableDictionary<int, PosInfo> specialIndexes = new();
     [SerializeField] private SerializableDictionary<int, Vector2> nextLine = new();
     
     //==================================================||Property
     
     public int Index { get; private set; } = 0;
 
    //==================================================||Method 
     
     public void TurnOn() {
         SetIndex(0);
     }
 
     public void SetIndex(int index) {

         this.Index = index;
         
         if (specialIndexes.TryGetValue(index, out PosInfo info)) {
             rect.localRotation = Quaternion.Euler(0, 0, info.Degree);
             rect.localPosition = info.Pos;
             return;
         }
         
         rect.localRotation = Quaternion.Euler(0, 0, defaultDegree + intervalDegree * index);
         Vector2 startPos = defaultPos;

         int nextLineIndex = 0;
         for (int i = 0; i <= index; i++) {

             if (nextLine.TryGetValue(i, out var temp)) {
                 nextLineIndex = i;
                 startPos = temp;
             }
         }
         
         rect.localPosition = startPos.ToVec3() + interval * (index - nextLineIndex);
     }

     public void Disappear() => transform.localPosition = new(2000,2000);
     
    //==================================================||Unity Func 
     private void Awake() {
         rect = GetComponent<RectTransform>();
     }       
}