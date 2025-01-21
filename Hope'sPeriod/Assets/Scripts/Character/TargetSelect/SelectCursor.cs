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

    public static SelectCursor Instance { get; private set; } = null;

    public SelectType SelectType { get; private set; }
    public int Index { get; private set; } = 0;
    public bool IsOn { get; private set; }= false;
    private SpriteRenderer renderer = null;
    private readonly Vector2 defaultPosition = new(-2, 1.7f);
    private readonly Vector2 selectInterval = new(5.86f, 0);

    private void UpdateIndex() {
        SetIndex(Index);
    }
    
    public void SetIndex(int index) {

        int count = ControleCharacterInfo.Instance.CharacterCount;
        if (index > count)
            throw new OutOfRange(0, count, index);

        this.Index = index;
        transform.localPosition = defaultPosition + index * selectInterval;
    }

    private void AddIndex(bool aliveOnly = false) {
        
        Index++;
        int count = ControleCharacterInfo.Instance.CharacterCount;
        if (Index >= count)
            Index = 0;

        if (aliveOnly) {

            while (!ControleCharacterInfo.Instance.Alive(Index)) {
                
                Index++;
                if (Index >= count)
                    Index = 0;
            }
        }
        
        UpdateIndex();
    }

    private void ExtractIndex(bool aliveOnly = false) {
        Index--;
        int count = ControleCharacterInfo.Instance.CharacterCount;
        if (Index < 0)
            Index = count - 1;

        if (aliveOnly) {

            while (!ControleCharacterInfo.Instance.Alive(Index)) {
                Index--;
                if (Index < 0)
                    Index = count - 1;
            }
        }
        UpdateIndex();
    }
    
    public void TurnOn(SelectType type) {

        SelectType = type;
        SetIndex(0);
        IsOn = true;
        renderer.enabled = true;
    }

    public void TurnOff() {
        
        IsOn = false;
        renderer.enabled = false;
    }
    
    private void Awake() {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {

        if (!IsOn)
            return;
        
        if (InputManager.Instance.Click(KeyTypes.Right)) {
            AddIndex();
        }

        if (InputManager.Instance.Click(KeyTypes.Left)) {
            ExtractIndex();
        }

        if (InputManager.Instance.Click(KeyTypes.Select)) {
            TurnOff();
        }
        
        else if (InputManager.Instance.Click(KeyTypes.Cancel)) {
            TurnOff();
            TargetButtonManager.Instance.Interactable = false;
            ItemListButtonManager.Instance.Interactable = true;
        }
    }
}