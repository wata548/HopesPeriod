using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class TutorialButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; } = false;
    [SerializeField] private TMP_Text page;
    [SerializeField] private GameObject window;
    [field: SerializeField] public SettingCategoryButtonManager manager { get; private set; }
    private const int ShowCount = 5;
    
    public override void SelectIn(InteractButton target) {}

    public override void SelectOut(InteractButton target) {}

    private int maxPage = 0;
    private int currentPage = 0;
    
    public void TurnOn() {
        window.SetActive(true);
        Interactable = true;

        int count = TutorialInfo.ShowedTutorial.Count;
        bool modulo = count % ShowCount != 0;
        maxPage = count / ShowCount + (modulo ? 1 : 0);
        currentPage = 0;
        if (count == 0)
            page.text = "<0/0>";
        else 
            page.text = $"<{currentPage + 1}/{maxPage}>";

        Refresh();
    }

    private void Refresh() {
        int startIndex = ShowCount * currentPage;

        int count = TutorialInfo.ShowedTutorial.Count;
        for (int i = 0; i < ShowCount; i++) {

            int index = startIndex + i;
            int code = 0;
            if (index >= count) index = -1;
            else code = TutorialInfo.ShowedTutorial.ElementAt(index);
            Parse(buttons[i]).SetInfo(index, code);
        }   
    }

    public void TurnOff() {
        
        window.SetActive(false);
        Interactable = false;
    }

    public void NextPage() {
        
        if (maxPage <= 1)
            return;        
        
        currentPage++;
        if (currentPage == maxPage) {
            currentPage = 0;
        }

        Refresh(); 
    }

    public void PriviousPage() {
        if (maxPage <= 1)
            return;
        
        currentPage--;
        if (currentPage == -1) {
            currentPage = maxPage - 1;
        }

        Refresh();
    }

    private TutorialButton Parse(InteractButton button) {
        if (button is not TutorialButton result)
            throw new TypeMissMatched(button.gameObject, typeof(TutorialButton));

        return result;
    }
    
    
}