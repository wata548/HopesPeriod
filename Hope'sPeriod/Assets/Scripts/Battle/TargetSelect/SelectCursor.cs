using System;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEditor.AddressableAssets.Build.BuildPipelineTasks;
using UnityEngine;

public enum SelectType {
    Players,
    EveryOne
}

public class SelectCursor: MonoBehaviour {

   //==================================================||Property 
    public static SelectCursor Instance { get; private set; } = null;
    public int Index { get; private set; } = 0;

   //==================================================||Field 
    private SpriteRenderer renderer = null;
    private readonly Vector2 defaultPosition = new(-2, 1.7f);
    private readonly Vector2 selectInterval = new(5.86f, 0);

   //==================================================||Method 
   
   public void TurnOn() {

       SetIndex(0);
       renderer.enabled = true;
   }

    public void TurnOff() {
        
        renderer.enabled = false;
    }
       
    private void UpdateIndex() {
        SetIndex(Index);
    }
    
    public void SetIndex(int index) {

        int count = CharactersInfoBattle.Instance.CharacterCount;
        if (index > count)
            throw new OutOfRange(0, count, index);

        this.Index = index;
        transform.localPosition = defaultPosition + index * selectInterval;
    }

    public void AddIndex(bool aliveOnly = false) {
        
        Index++;
        int count = CharactersInfoBattle.Instance.CharacterCount;
        if (Index >= count)
            Index = 0;

        if (aliveOnly) {

            while (!CharactersInfoBattle.Instance.Alive(Index)) {
                
                Index++;
                if (Index >= count)
                    Index = 0;
            }
        }
        
        UpdateIndex();
    }

    public void ExtractIndex(bool aliveOnly = false) {
        Index--;
        int count = CharactersInfoBattle.Instance.CharacterCount;
        if (Index < 0)
            Index = count - 1;

        if (aliveOnly) {

            while (!CharactersInfoBattle.Instance.Alive(Index)) {
                Index--;
                if (Index < 0)
                    Index = count - 1;
            }
        }
        UpdateIndex();
    }
    
    
   //==================================================||Unity Func 
    private void Awake() {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        renderer = GetComponent<SpriteRenderer>();
    }
}