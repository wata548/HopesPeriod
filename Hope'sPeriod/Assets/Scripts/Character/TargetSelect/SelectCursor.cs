using System;
using System.Dynamic;
using UnityEngine;

public class SelectCursor: MonoBehaviour {

    public static SelectCursor Instance { get; private set; } = null;

    private int index = 0;
    private bool isOn = false;
    private SpriteRenderer renderer = null;
    private readonly Vector2 defaultPosition = new(-2, 1.7f);
    private readonly Vector2 selectInterval = new(5.86f, 0);

    private void UpdateIndex() {
        SetIndex(index);
    }
    
    public void SetIndex(int index) {

        int count = ControleCharacterInfo.Instance.CharacterCount;
        if (index > count)
            throw new OutOfRange(0, count, index);

        this.index = index;
        transform.localPosition = defaultPosition + index * selectInterval;
    }

    private void AddIndex(bool aliveOnly = false) {
        
        index++;
        int count = ControleCharacterInfo.Instance.CharacterCount;
        if (index >= count)
            index = 0;

        if (aliveOnly) {

            while (!ControleCharacterInfo.Instance.Alive(index)) {
                
                index++;
                if (index >= count)
                    index = 0;
            }
        }
        
        UpdateIndex();
    }

    private void ExtractIndex(bool aliveOnly = false) {
        index--;
        int count = ControleCharacterInfo.Instance.CharacterCount;
        if (index < 0)
            index = count - 1;

        if (aliveOnly) {

            while (!ControleCharacterInfo.Instance.Alive(index)) {
                index--;
                if (index < 0)
                    index = count - 1;
            }
        }
        UpdateIndex();
    }
    
    private void TurnOn() {
        
        index = 0;
        isOn = true;
        renderer.enabled = true;
    }

    private void TurnOff() {
        
        isOn = false;
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
        if (Input.GetKeyDown(KeyCode.A)) {
            ExtractIndex();
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            AddIndex();
        }
    }
}