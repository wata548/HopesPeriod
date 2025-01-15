using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemButton: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private FloatingItemInfo tempItemInfo = null; 
    private static FloatingItemInfo itemInfo = null;
    private RectTransform rect;
    private bool onMouse = false;
    
    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("in");
        onMouse = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("out"); 
        onMouse = false;
    }

    private void Awake() {

        if (itemInfo is null && tempItemInfo is not null)
            itemInfo = tempItemInfo;
        
        rect = GetComponent<RectTransform>();
    }

    private void Update() {

        if (itemInfo is not null && onMouse) {

            itemInfo.UpdatePivot(rect.position);
            itemInfo.UpdatePosition();
        }
    }
}