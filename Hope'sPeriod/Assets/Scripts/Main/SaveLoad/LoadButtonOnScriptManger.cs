using TMPro;
using UnityEngine;

public class LoadButtonOnScriptManger: LoadButtonOnPlayManager {

    public static LoadButtonOnScriptManger Instance { get; private set; } = null;

    [SerializeField] private GameObject background;
    
    public void Awake() {
        base.Awake();
        Instance = this;
    }

    public void TurnOn(string title) {
        
        base.TurnOn();

        background.SetActive(true);
        var titleContext = base.title.GetComponent<TMP_Text>();
        titleContext.text = "저장";
        if (!string.IsNullOrEmpty(title))
            titleContext.text += $"({title})";
        
    }

    public void TurnOff() {
        background.SetActive(false);
        base.TurnOff();
    }
}