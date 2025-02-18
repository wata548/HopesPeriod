using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItemWindow: MonoBehaviour {

    public static GetItemWindow Instance { get; private set; } = null;
    private static readonly Color Transparent = new Color(1,1,1, 0);
    
    [SerializeField] private Image window;
    private Color windowColor;
    [SerializeField] private Image line;
    [SerializeField] private Image itemImage;
    [SerializeField] private WaveMovementButton cursor;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;

    private bool on = false;
    private const float StartAppearPoint = -130f;
    private const float CompleteAppearPoint = -20f;
    private const float AppearTime = 0.55f;
    private const float MoveTime = 0.6f;

    public bool Off() => !on;
    
    public void TurnOff() {
        on = false;
        
        line.color = Transparent;
        window.color = windowColor;
        window.DOFade(0, 0);
        itemImage.color = Transparent;
        cursor.gameObject.SetActive(false);
        cursor.TurnOff();
        itemName.color = Transparent;
        itemDescription.color = Transparent;

        window.transform.localPosition = new(0, StartAppearPoint);
        TilePlayerPhysics.SetMovable(true);
    }

    public void TurnOn(GetItemInfo itemInfo) {

        Inventory.AddItem(itemInfo.Code, itemInfo.Count);
        
        on = true;
        line.DOFade(1, AppearTime);
        window.DOFade(0.5f, AppearTime);
        window.transform.DOLocalMoveY(CompleteAppearPoint, MoveTime)
            .SetEase(Ease.OutBack)
            .OnComplete(() => {
                cursor.gameObject.SetActive(true);
                cursor.TurnOn();
            });
        
        itemImage.sprite = CodeInfo.LoadImage(itemInfo.Code);
        itemImage.DOFade(1, AppearTime);
        itemName.DOFade(1, AppearTime);
        itemName.text = ItemInfo.Name(itemInfo.Code) + $" ×{itemInfo.Count}".SetSize(0.75f);
        itemDescription.DOFade(1, AppearTime);
        itemDescription.text = ItemInfo.Description(itemInfo.Code);
        TilePlayerPhysics.SetMovable(false);
    }
    
    private void Awake() {
        windowColor = window.color;
        windowColor.a = 0.5f;

        Instance = this;
    }

    private void Start() {
       
        TurnOff();
    }

    private void Update() {

        if (!on)
            return;

        if (InputManager.Instance.Click(KeyTypes.Interaction)) {

            TurnOff();
        } 
    }
}