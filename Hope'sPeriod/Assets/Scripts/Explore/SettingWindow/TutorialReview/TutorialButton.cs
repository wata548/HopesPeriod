using System;
using TMPro;
using UnityEngine;

public class TutorialButton: InteractButtonUI {

    [SerializeField] private TMP_Text context;
    private int code;
    public bool On { get; private set; } = false;
    
    public void SetInfo(int index, int code) {

        this.code = code;
        
        if (index == -1) {
            context.text = "";
            On = false;

            return;
        }

        string title = TutorialInfo.Title(code);
        On = true;
        context.text = $"{$"{index + 1}.".SetSize(1.3f)} {title}";
    }
    
    public override void Click() {

        if (!Manager.Interactable)
            return;
        
        if (!On)
            return;

        Parse(Manager).manager.SetInteractable(false);
        SettingWindow.SetInteractable(false);
        Manager.SetInteractable(false);
        
        ScriptShower.Instance.ShowTutorial(code);

        StartCoroutine(Wait.WaitAndDo(() => ScriptShower.Instance.EventCode == 0, () => {
            SettingWindow.SetInteractable(true);
            Manager.SetInteractable(true);
            Parse(Manager).manager.SetInteractable(true);
        }));
    }

    private TutorialButtonManager Parse(InteractButtonManager manager) {

        if (manager is not TutorialButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(TutorialButtonManager));

        return result;
    }
}