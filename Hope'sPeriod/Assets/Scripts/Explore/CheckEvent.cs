using System;
using System.Numerics;
using UnityEngine;
using Object = UnityEngine.Object;
using Image = UnityEngine.UI.Image;
using DG.Tweening;
using Vector2 = UnityEngine.Vector2;

public static class CheckEvent {

    private static readonly Vector2 DefaultPos = new(0, 0.13f);

    private static Image mapMoveEffect = null;
    private static GameObject map = null;
    private static MapEventInfo mapInfo = null;
    private static int mapCode;

    public static void SetEffect(Image effect) => mapMoveEffect = effect;
    public static void SetMap(int code) {
        mapCode = code;
        mapInfo = Resources.Load<MapEventInfo>("MapPrefab/MapInfo");
        map = Object.Instantiate(mapInfo.Prefab(code));
    }
    
    public static void CheckAutoEvent(ref Vector2Int pos, GameObject player) {

        if (mapInfo.ConnectInfo(mapCode, pos, out ConnectMapInfo connectMapInfo, out GameObject mapPrefab)) {
        
            if(map is not null)
                Object.Destroy(map);
            
            Debug.Log(mapPrefab);
            map = Object.Instantiate(mapPrefab);
            mapCode = connectMapInfo.ConnectMapCode;
            player.transform.localPosition = DefaultPos + connectMapInfo.ConnectPos;
            pos = connectMapInfo.ConnectPos;
                            
            mapMoveEffect.color = Color.black;
            mapMoveEffect.DOFade(0, 0.7f).SetEase(Ease.InCubic);
        }
        else if (mapInfo.AutoInfo(mapCode, pos, out int code)) {
            Debug.Log($"auto event {code}");
        }
    }

    public static void CheckInteract(Vector2Int pos, Direction viewDirection) {
        
        if (InputManager.Instance.Click(KeyTypes.Interaction)) {
                
            var direction = viewDirection.ConvertVector().ToVec2Int();
        
            if (mapInfo.Item(mapCode, pos, out int itemCode)) {
                Debug.Log($"Get item {ItemInfo.Name(itemCode)} at current pos");
            }
            else if (mapInfo.Item(mapCode, pos + direction, out itemCode)) {
                Debug.Log($"Get item {ItemInfo.Name(itemCode)} at view point");
            }
            else if (mapInfo.PassiveInfo(mapCode, pos, out int interactCode)) {
                Debug.Log($"?");
            }
        }
    }
}