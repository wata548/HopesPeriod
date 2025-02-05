using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton: InteractButtonUI {

    [SerializeField] private Image backGround;
    [SerializeField] private TMP_Text buttonName;
    public override void Click() {
        if (!Manager.Interactable) return;
        Parse(Manager).SetSelect(Index);
    }

    public void SetColor(Color color) {
        
        backGround.color = color;
        buttonName.color = color;
    }

    private CategoryButtonManager Parse(InteractButtonManager manager) {
        if (manager is not CategoryButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(CategoryButtonManager));

        return result;
    }
}
