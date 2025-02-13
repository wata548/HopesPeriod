using UnityEngine;

public class CharacterSelectButtonManager: InteractButtonManager {
    public override bool Interactable { get; protected set; } = true;
    [SerializeField] private EachChararacterInfoInventory[] infos;
    [SerializeField] private GameObject characterList;
    public override void SelectIn(InteractButton target) { }

    public override void SelectOut(InteractButton target) { }

    public void TurnOn() {
        characterList.SetActive(true);
        Interactable = true;
        foreach (var button in buttons) {
            Parse(button).TurnOn();
        }
        foreach (var info in infos) {
            info.TurnOn();
        }
    }

    public void TurnOff() {
        characterList.SetActive(false);
        Interactable = false;
    }

    private void Awake() {
        base.Awake();

        int index = 0;
        foreach (var button in buttons) {
            Parse(button).SetInfo(infos[index++].Info);
        }
    }

    private CharacterSelectButton Parse(InteractButton button) {
        if (button is not CharacterSelectButton result)
            throw new TypeMissMatched(button.gameObject, typeof(CharacterSelectButton));
        return result;
    }
    
}