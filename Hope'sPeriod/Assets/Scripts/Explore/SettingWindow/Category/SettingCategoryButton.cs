using System;
using TMPro;

public class SettingCategoryButton: InteractButtonUI {

    private TMP_Text context;

    private void Awake() {

        context = GetComponentInChildren<TMP_Text>();
    }

    public override void Click() {

        Parse(Manager).Click(Index);
    }

    private SettingCattegoryButtonManager Parse(InteractButtonManager manager) {
        if (manager is not SettingCattegoryButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(SettingCattegoryButtonManager));

        return result;
    }
}