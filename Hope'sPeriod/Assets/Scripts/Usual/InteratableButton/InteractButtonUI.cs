public abstract class InteractButtonUI : InteractButton {

    protected void ClickSound() {
        if (Manager.UseClickSound)
            AudioManager.Instance.PlayOne(FmodEvents.Instance.Click);
    }
}