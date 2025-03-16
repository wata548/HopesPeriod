using System;

public class MainButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; } = true;
    
    public override void SelectIn(InteractButton target) {
        Parse(target).Active();
        AudioManager.Instance.PlayOne(FmodEvents.Instance.SelectIn);
    }

    public override void SelectOut(InteractButton target) {
        Parse(target).Disactive();
    }

    public void Init() {
        foreach (var button in buttons) {
            SelectOut(button);
        }
    }

    private MainButton Parse(InteractButton button) {
        if (button is not MainButton result)
            throw new TypeMissMatched(button.gameObject, typeof(MainButton));

        return result;
    }
}