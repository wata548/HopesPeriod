using System;
using TMPro;

public class SettingCategoryButton: InteractButtonUI {

    private TMP_Text context;

    private void Awake() {

        context = GetComponentInChildren<TMP_Text>();
    }

    public override void Click() {

        
    }

    
}