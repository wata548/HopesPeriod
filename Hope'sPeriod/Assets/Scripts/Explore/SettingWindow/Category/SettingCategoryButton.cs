using System;
using TMPro;
using UnityEngine;

public class SettingCategoryButton: InteractButtonUI {

    private TMP_Text context;
    private static TMP_Text beforeSelect = null;
    
    private void Awake() {

        context = GetComponentInChildren<TMP_Text>();
    }

    public override void Click() {

        if(beforeSelect is not null)
            beforeSelect.color = Color.white;
        context.color = Color.yellow;
        
        if (!Manager.Interactable)
            return;
        
        Parse(Manager).Click(Index);
    }

    private SettingCategoryButtonManager Parse(InteractButtonManager manager) {
        if (manager is not SettingCategoryButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(SettingCategoryButtonManager));

        return result;
    }
}