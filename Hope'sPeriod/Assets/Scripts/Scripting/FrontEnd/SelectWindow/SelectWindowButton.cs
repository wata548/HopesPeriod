using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectWindowButton: InteractButtonUI {
    
    private static readonly Color active = Color.yellow;
    private static readonly Color disactive = Color.clear;
    
    
    public override void Click() {
        ToSelectWindow(Manager).TurnOff(Index);
    }

    private TMP_Text context;
    private RectTransform rect;
    private Image background;
    public void Active() {
        context.color = Color.black;
        background.color = active;
    }

    public void Disactive() {
        context.color = Color.white;
        background.color = disactive;
    }

    public void SetContext(string info, Vector2 pos) {
        
        rect = GetComponent<RectTransform>();
        background = GetComponent<Image>();
        context = GetComponentInChildren<TMP_Text>();
        
        rect.anchoredPosition = pos;
        
        context.text = info;
    }
    
    private static SelectWindow ToSelectWindow(InteractButtonManager manager) {
        if (manager is not SelectWindow result)
            throw new TypeMissMatched(manager.gameObject, typeof(SelectWindow));

        return result;
    }
}