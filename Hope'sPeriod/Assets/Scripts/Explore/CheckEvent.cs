using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Object = UnityEngine.Object;
using Image = UnityEngine.UI.Image;
using DG.Tweening;
using Unity.VisualScripting;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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
        //SetItem(code);
        
        map = Object.Instantiate(mapInfo.Prefab(code));
    }

    public static int MeetMonster() {

        LoadMapInfo();
        return mapInfo.MeetMonster(mapCode);
    }

    public static bool CheckAutoEvent(ref Vector2Int pos, GameObject player) {

        LoadMapInfo();
        bool result = MoveEvent(ref pos, player);
        
        //Didn't move map
        if(!result) moveMap = false;
        AutoEvent(pos);

        return result;
    }

    public static bool MoveEvent(ref Vector2Int pos, GameObject player) {
        if (mapInfo.ConnectInfo(mapCode, pos, out ConnectMapInfo connectMapInfo, out GameObject mapPrefab)) {
        
            if (moveMap) return false;
        
            if (map is not null)
                Object.Destroy(map);
        
            Debug.Log(mapCode);
            map = Object.Instantiate(mapPrefab);
            mapCode = connectMapInfo.ConnectMapCode;
            //SetItem(mapCode);
            player.transform.localPosition = DefaultPos + connectMapInfo.ConnectPos;
            pos = connectMapInfo.ConnectPos;
        
            mapMoveEffect.color = Color.black;
            mapMoveEffect.DOFade(0, 0.7f).SetEase(Ease.InCubic);
            moveMap = true;
            return true;
        }

        return false;
    }
    
    public static void AutoEvent(Vector2Int pos) {
        if (mapInfo.AutoInfo(mapCode, pos, out var info)) {
            StartEvent(info.Code);
        }
    }

    public static void StartEvent(int code) {
        var codeType = code.ToCodeType();
        switch (codeType) {
            case CodeType.Script:
                ScriptShower.Instance.StartScript(code);
                break;
            case CodeType.Tutorial:
                ScriptShower.Instance.ShowTutorial(code);
                break;
        }
    }

    public static void CheckInteract(Vector2Int pos, Direction viewDirection) {

        LoadMapInfo();
        if (InputManager.Instance.Click(KeyTypes.Interaction)) {
                
            var direction = viewDirection.ConvertVector().ToVec2Int();
        
            if (mapInfo.Item(mapCode, pos, out var item)) {
                //SetItem(mapCode);
                Debug.Log($"Get item {ItemInfo.Name(item.Code)} * {item.Count} at current pos");
                GetItemWindow.Instance.TurnOn(item);
            }
            else if (mapInfo.Item(mapCode, pos + direction, out item)) {
                //SetItem(mapCode);
                Debug.Log($"Get item {ItemInfo.Name(item.Code)} * {item.Count} at view point");
                GetItemWindow.Instance.TurnOn(item);
            }
            else if (mapInfo.PassiveInfo(mapCode, pos, out var info)) {
                StartEvent(info.Code);
            }
        }
    }

    private static Dictionary<Vector3Int, GameObject> itemObjects = new();
    public static void SetItem(int mapCode) {
        
        foreach (var itemObject in itemObjects) {
            GameObject.Destroy(itemObject.Value);
        }
        itemObjects.Clear();
        
        var items = mapInfo.Items(mapCode);

        foreach (var item in items) {
            bool check = ScriptCodeInterpreter
                .Interpret(item.Item2.Condition)
                .ToCondition();
            
            if(!check) 
                continue;

            var newItem = new GameObject();
            var renderer = newItem.AddComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>($"CodeImage/Item/{item.Item2.Code}");
            renderer.sortingLayerID = SortingLayer.NameToID("Item");
            newItem.transform.localPosition = item.Item1;
            newItem.transform.localScale = new(0.5f, 0.5f);

            itemObjects.Add(item.Item1, newItem);
        }
    }
}