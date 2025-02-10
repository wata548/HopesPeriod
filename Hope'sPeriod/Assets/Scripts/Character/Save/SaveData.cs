using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class SaveFormat {
    public SaveCharacterInfo[] SaveCharacterInfo;
    public SaveItem[] SaveItem;
    public SavePos SavePos;
    public SaveMonster[] SaveMonster;
    public SaveFindEvent[] SaveFindEvent;
    public SaveFindItem[] SaveFindItem;
}

public class SaveItem {
    [JsonProperty]
    public int Code { get; private set; }
    [JsonProperty]
    public int Amount { get; private set; }

    public SaveItem() {}
    public SaveItem(int code, int amount) {
        Code = code;
        Amount = amount;
    }
}
public class SaveMonster {
    [JsonProperty]
    public int Code { get; private set; }
    [JsonProperty]
    public int KillCount { get; private set; }

    public SaveMonster() {}
    public SaveMonster(int code, int killCount) {
        Code = code;
        KillCount = killCount;
    }
}
public class SavePos {
    [JsonProperty]
    public int Code { get; private set; }
    [JsonProperty]
    public int X { get; private set; }
    [JsonProperty]
    public int Y { get; private set; }

    public SavePos() {}
    public SavePos(int code, Vector2Int pos) {
        Code = code;
        X = pos.x;
        Y = pos.y;
    }
}
public class SaveFindItem {
    [JsonProperty]
    public int Code { get; private set; }
    [JsonProperty]
    public int X { get; private set; }
    [JsonProperty]
    public int Y { get; private set; }

    public SaveFindItem() {}
    public SaveFindItem(int code, Vector2Int pos) {
        Code = code;
        X = pos.x;
        Y = pos.y;
    }
}
public class SaveFindEvent {
    [JsonProperty]
    public int Code { get; private set; }
    [JsonProperty]
    public int X { get; private set; }
    [JsonProperty]
    public int Y { get; private set; }

    public SaveFindEvent() {}

    public SaveFindEvent(int code, Vector2Int pos) {
        Code = code;
        X = pos.x;
        Y = pos.y;
    }
}
public class SaveCharacterInfo {
    [JsonProperty]
    public string Name { get; private set; }
    [JsonProperty]
    public bool Exist;
    [JsonProperty]
    public bool Dead;
    [JsonProperty]
    public int[] Skill { get; private set; }
    [JsonProperty]
    public int[] HaveSkill { get; private set; }
    [JsonProperty]
    public float MaximumHp { get; private set; }
    [JsonProperty]
    public float CurrentHp { get; private set; }
    [JsonProperty]
    public float MaximumMp { get; private set; }
    [JsonProperty]
    public float CurrentMp { get; private set; }

    public SaveCharacterInfo() {}

    public SaveCharacterInfo(EachCharacterInfo playerInfo) {

        Name = Regex.Match(playerInfo.gameObject.name, @"(.*)Info").Groups[1].Value;
        Exist = playerInfo.Exist;
        Dead = playerInfo.Dead;
        Skill = playerInfo.Skill;
        HaveSkill = playerInfo.Skill.ToArray();
        MaximumHp = playerInfo.MaximumHp;
        CurrentHp = playerInfo.CurrentHp;
        MaximumMp = playerInfo.MaximumMp;
        CurrentMp = playerInfo.CurrentMp;
    }
}

public class SaveData {

    public static SaveFormat Load(string name) {
        string data = File.ReadAllText(Application.streamingAssetsPath + $@"/SaveFile/{name}.json"); 
        return JsonConvert.DeserializeObject<SaveFormat>(data);
    }
    
    public static void Save(EachCharacterInfo[] playerInfos, int mapCode, Vector2Int pos, int saveSlot) {
 
        var json = new JObject();
        SerializeCharacter(json, playerInfos);
        SerializeInventory(json);
        SerializePos(json, mapCode, pos);
        SerializeMonster(json);
        SerializeFindEvent(json);
        SerializeFindItem(json);
        File.WriteAllText(Application.streamingAssetsPath + $@"/SaveFile/SaveData{saveSlot}.json", json.ToString(Formatting.Indented));
    }

    private static void SerializeCharacter(JObject json, EachCharacterInfo[] playerInfos) {
        var fixInfos = playerInfos.Select(info => new SaveCharacterInfo(info));
        var jPlayerInfo = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SaveCharacterInfo), jPlayerInfo);
    }

    private static void SerializeInventory(JObject json) {
        var fixInfos = Inventory.Items.Select(item => new SaveItem(item.Key, item.Value));
        var jInfo = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SaveItem), jInfo);
    }

    private static void SerializePos(JObject json, int code, Vector2Int pos) {
        var fixInfos = new SavePos(code, pos);
        JObject parse = JObject.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SavePos), parse);
    }

    private static void SerializeMonster(JObject json) {
        var fixInfos = MonsterInfo.Monsters.Select(monster => new SaveMonster(monster.Key, monster.Value));
        var parse = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SaveMonster), parse);
    }
    
    private static void SerializeFindItem(JObject json) {
        var fixInfos = FindEventInfo.FindItemInfos
            .Select(findItem => {
                var items = findItem.Value
                    .Select(factor => new SaveFindItem(findItem.Key, factor));
                return items.ToList();
            })
            .SelectMany(factor => factor);
        var parse = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SaveFindItem), parse);
    }
    private static void SerializeFindEvent(JObject json) {
        var fixInfos = FindEventInfo.FindEventInfos
            .Select(findEvent => {
                var events = findEvent.Value
                    .Select(factor => new SaveFindEvent(findEvent.Key, factor));
                return events.ToList();
            })
            .SelectMany(factor => factor);
        var parse = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SaveFindEvent), parse);
    }
    
}