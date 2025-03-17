using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SelectWindow: InteractButtonManager {

    public static SelectWindow Instance { get; private set; } = null;
    
    [SerializeField] private GameObject outline;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject elementPrefab;
    [SerializeField] private TMP_Text title;
    private RectTransform backgroundShowerRect = null;
    private List<int> connectEvent;
    
    private const float defaulHeight = 30;
    private const float titleHeight = 90;
    private const float titleInterval = 68;
    private const float elementInterval = 60;
    private const float backgroundWidth = 240;
    private int cursorTarget = -1;
    public static int Select { get; private set; } = -1;
    
    public override bool Interactable { get; protected set; } = false;

    public int EndEvent()
        => connectEvent[Select];
    
    public void TurnOff(int select) {
        cursorTarget = -1;
        
        Interactable = false;
        outline.SetActive(false);
        title.gameObject.SetActive(false);
        SelectWindow.Select = select;
    }
    
    public override void SelectIn(InteractButton target) {
        if (target.Index == cursorTarget)
            return;
        
        ToSelectWindowButton(target).Active();

        if(cursorTarget != -1) {
            ToSelectWindowButton(buttons[cursorTarget]).Disactive();
        }
        cursorTarget = target.Index;
    }

    public override void SelectOut(InteractButton target) { }

    private SelectWindowButton ToSelectWindowButton(InteractButton button) {
        if (button is not SelectWindowButton result)
            throw new TypeMissMatched(button.gameObject, typeof(SelectWindowButton));
        return result;
    }
    
    private void Awake() {
        Instance = this;
        UseClickSound = false;
        UseSelectSound = true;
    }

    public void ShowSelect(List<string> element,List<int> connectEvent, string titleName = "") {

        foreach (var elementData in element)
            Debug.Log(elementData);
        foreach (var factor in connectEvent) {
            Debug.Log(factor);
        }
        
        //init
        outline.SetActive(true);
        this.connectEvent = connectEvent;
        
        Interactable = true;
        foreach (var button in buttons) {
            Destroy(button.gameObject);
        }

        buttons.Clear();
        
        //some setting
        bool useTitle = !string.IsNullOrEmpty(titleName);
        float yPos = 0;
        int size = element.Count;
        float height = size * elementInterval + defaulHeight; 
        
        //title setting
        if (useTitle) {

            title.gameObject.SetActive(true);
            title.text = titleName;
            yPos = -titleInterval;

            height += titleHeight;
        }
        else {
            title.gameObject.SetActive(false);
        }

        //backboard setting
        background.GetComponent<RectTransform>().sizeDelta = new(backgroundWidth, height);
        outline.GetComponent<RectTransform>().sizeDelta = new(backgroundWidth + 10, height + 10);

        //make element
        for (int i = 0; i < size; i++) {

            yPos -= elementInterval;
            
            var newElement = Instantiate(elementPrefab, background.transform);
            
            var button = newElement.GetComponent<SelectWindowButton>();
            button.SetContext(element[i], new(0, yPos));
            buttons.Add(button);
        }

        //each element's index Setting
        SetIndex();
    }
}