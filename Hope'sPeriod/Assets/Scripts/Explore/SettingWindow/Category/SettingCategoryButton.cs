using System;
using TMPro;
using UnityEngine;

public class SettingCategoryButton: InteractButtonUI {

    public TMP_Text Context { get; private set; }
    private static TMP_Text beforeSelect = null;
    
    private void Awake() {

        Context = GetComponentInChildren<TMP_Text>();
    }

    public static void SetColor(TMP_Text target) {
        if(beforeSelect is not null)
            beforeSelect.color = Color.white;
        target.color = Color.yellow;
        beforeSelect = target;
    }
    
    public override void Click() {
        
        if (!Manager.Interactable)
            return;
        
        SetColor(Context);
        ClickSound();
        Parse(Manager).Click(Index);
    }

    private SettingCategoryButtonManager Parse(InteractButtonManager manager) {
        if (manager is not SettingCategoryButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(SettingCategoryButtonManager));

        return result;
    }
}