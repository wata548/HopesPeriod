using TMPro;
using UnityEngine;

public class UseButton: InteractButtonUI {

    [SerializeField] private TMP_Text text;
    public TMP_Text Text => text;
    
    public void SetColor(Color color) => text.color = color; 
    public override void Click() {

        if (!Manager.Interactable) return;
        var manager = Parse(Manager);
        
        switch (Index) {
            case 0:
                
                int code = Parse(Manager).Code;
                if (Inventory.Count(code) <= 0)
                    break;
                
                if (ItemInfo.HealItemNeedSelect(code)) {
                    
                    manager.TargetButtonManager.TurnOn(code);
                    break;
                }
                
                Inventory.UseItem(code);
                CharacterInfoInventory.Instance.UpdateState();
                manager.UpdateInfo();
               
                break;
            case 1:
                manager.SelectOut(this);
                manager.SetEmpty();
                manager.SetKeBoardControlable(false);
                break;
        }
    }

    public UseButtonManager Parse(InteractButtonManager manager) {
        if (manager is not UseButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(UseButtonManager));

        return result;
    } 
}