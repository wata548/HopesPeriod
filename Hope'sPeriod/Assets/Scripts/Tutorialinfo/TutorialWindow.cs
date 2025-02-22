using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour {

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text pageText;
    [SerializeField] private TMP_Text nextButtonText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject priviousButton;
    [SerializeField] private GameObject window;
    [SerializeField] private Image imageInfo;
    [SerializeField] private TMP_Text imageTypeContext;
    [SerializeField] private TMP_Text context;

    public static TutorialWindow Instance { get; private set; } = null; 
    private List<TutorialScriptCommand> tutorialData;
    private int currentPage;

    public bool On { get; private set; } = false;

    private void TurnOn() {

        window.SetActive(true);
        On = true;
    }

    public void SetTutorial(List<TutorialScriptCommand> list) {
        tutorialData = list;
        currentPage = 0;
        TurnOn();
        NextPage();
    }

    public void NextPage() {
        if (tutorialData.Count == currentPage) {
            
            TurnOff();
            return;
        }

        SetTutorial(tutorialData[currentPage++]);
    }

    public void PriviousPage() {
        if (currentPage <= 1)
            return;
        
        currentPage--;
        SetTutorial(tutorialData[currentPage - 1]);
    }
    
    private void SetTutorial(TutorialScriptCommand info) {

        if (tutorialData.Count == 1) {
            pageText.text = "";
        }
        else {
            pageText.text = $"{currentPage}/{tutorialData.Count}";
        }
        
        if (currentPage == 1)
            priviousButton.SetActive(false);
        else
            priviousButton.SetActive(true);
        
        if (tutorialData.Count == currentPage)
            nextButtonText.text = "끝내기";
        else
            nextButtonText.text = "다음으로";
        
        title.text = info.Title;
        string contextInfo = info.Info.Replace('{', '(').Replace('}', ')');
        if (string.IsNullOrEmpty(info.Image)) {
            
            context.text = contextInfo;
            imageTypeContext.text = "";
            imageInfo.color= new(0,0,0,0);
        }
        else {

            context.text = "";
            imageTypeContext.text = contextInfo;
            imageInfo.color = Color.white;
            imageInfo.sprite = Resources.Load<Sprite>(info.Image);
        }
    }
    
    private void TurnOff() {
        
        window.SetActive(false);
        On = false;
    }

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (!On)
            return;

        if (InputManager.Instance.ClickAndHold(KeyTypes.Select)) 
            NextPage();
        else if (InputManager.Instance.ClickAndHold(KeyTypes.Cancel))
            PriviousPage();
    }
}