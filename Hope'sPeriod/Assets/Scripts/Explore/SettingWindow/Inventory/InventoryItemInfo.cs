using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfo: MonoBehaviour {

    [SerializeField] private Image symbol;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemTag;
    [SerializeField] private TMP_Text itemDescription;

    public void SetInfo(int code) {

        if (code == 0) {
            SetEmpty();
            return;
        }
        
        symbol.color = Color.white;
        //symbol.sprite = Resources.Load<Sprite>($"ItemImage/{code}");
        itemName.text = ItemInfo.Name(code);
        itemTag.text = ItemInfo.SimpleTag(code);
        itemDescription.text = ItemInfo.Description(code);
    }

    public void SetEmpty() {
        symbol.color = new(0, 0, 0, 0);
        itemName.text = "";
        itemTag.text = "";
        itemDescription.text = "";
    }
}
