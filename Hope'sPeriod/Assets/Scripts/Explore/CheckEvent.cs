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
    private static bool moveMap = false;
    public static int MapCode => mapCode;
    public static string MapName => mapInfo.Name(mapCode);
    
    public static void SetEffect(Image effect) => mapMoveEffect = effect;

    private static void LoadMapInfo() {
        mapInfo ??= Resources.Load<MapEventInfo>("MapPrefab/MapInfo");
    }
    public static void SetMap(int code) {
        mapCode = code;
        LoadMapInfo();
        map = Object.Instantiate(mapInfo.Prefab(code));
    }

    public static int MeetMonster() {

        LoadMapInfo();
        return mapInfo.MeetMonster(mapCode);
    }

    public static bool CheckAutoEvent(ref Vector2Int pos, GameObject player) {

        LoadMapInfo();
        if (mapInfo.ConnectInfo(mapCode, pos, out ConnectMapInfo connectMapInfo, out GameObject mapPrefab)) {

            if (moveMap) return false;

            if (map is not null)
                Object.Destroy(map);

            Debug.Log(mapPrefab);
            map = Object.Instantiate(mapPrefab);
            mapCode = connectMapInfo.ConnectMapCode;
            player.transform.localPosition = DefaultPos + connectMapInfo.ConnectPos;
            pos = connectMapInfo.ConnectPos;

            mapMoveEffect.color = Color.black;
            mapMoveEffect.DOFade(0, 0.7f).SetEase(Ease.InCubic);
            moveMap = true;
            return true;
        }
        
        //Didn't move map
        moveMap = false;
        if (mapInfo.AutoInfo(mapCode, pos, out var info)) {
            Debug.Log($"auto event {info.Code}");
        }

        return false;
    }

    public static void CheckInteract(Vector2Int pos, Direction viewDirection) {

        LoadMapInfo();
        if (InputManager.Instance.Click(KeyTypes.Interaction)) {
                
            var direction = viewDirection.ConvertVector().ToVec2Int();
        
            if (mapInfo.Item(mapCode, pos, out var item)) {
                Debug.Log($"Get item {ItemInfo.Name(item.Code)} * {item.Count} at current pos");
                GetItemWindow.Instance.TurnOn(item);
            }
            else if (mapInfo.Item(mapCode, pos + direction, out item)) {
                Debug.Log($"Get item {ItemInfo.Name(item.Code)} * {item.Count} at view point");
                GetItemWindow.Instance.TurnOn(item);
            }
            else if (mapInfo.PassiveInfo(mapCode, pos, out var info)) {
                Debug.Log($"?");
            }
        }
    }
}