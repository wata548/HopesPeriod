
using FMODUnity;
using UnityEngine;

public class MainButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; } = true;
    
    [Header("Audio Sources")]
    [SerializeField] private EventReference hoverSound;
    
    
    public override void SelectIn(InteractButton target) {
        Parse(target).Active();
        AudioManager.Instance.PlayOne(hoverSound);
    }

    public override void SelectOut(InteractButton target) {
        Parse(target).Disactive();
    }

    private MainButton Parse(InteractButton button) {
        if (button is not MainButton result)
            throw new TypeMissMatched(button.gameObject, typeof(MainButton));

        return result;
    } 
}