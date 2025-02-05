public class InventoryTargetButton: InteractButton {
    public override void Click() {

        if (!Manager.Interactable) return;
        
        var manager = Parse(Manager);
        int code = manager.Code;
        var target = CharacterInfoInventory.Instance.GetCharacterInfo(Index).Info;
        if (!ItemInfo.UseAble(code, target))
            return;
        
        Inventory.UseItem(code, target);
        CharacterInfoInventory.Instance.UpdateState();
        manager.ItemInfo.UpdateInfo();
        
        manager.TurnOff();

    }

    private InventoryTargetButtonManager Parse(InteractButtonManager manager) {
        if (manager is not InventoryTargetButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(InventoryButtonManager));

        return result;
    }
}