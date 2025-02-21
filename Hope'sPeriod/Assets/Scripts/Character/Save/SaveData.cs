using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SaveFormat {
    public float PlayTime;
    public SaveCharacterInfo[] SaveCharacterInfo;
    public SaveItem[] SaveItem;
    public SavePos SavePos;
    public SaveMonster[] SaveMonster;
    public SaveFindEvent[] SaveFindEvent;
    public SaveFindItem[] SaveFindItem;
    public SaveUseItem[] SaveUseItem;
    public List<int> SaveEvent;
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
    public float X { get; private set; }
    [JsonProperty]
    public float Y { get; private set; }
   [JsonProperty] 
   public int Chapter { get; private set; }

    public SavePos() {}
    public SavePos(int code, Vector2 pos, int chapter) {
        Code = code;
        X = pos.x;
        Y = pos.y;
        Chapter = chapter;
    }
}
public class SaveFindItem {
    [JsonProperty]
    public int Code { get; private set; }
    [JsonProperty]
    public int X { get; private set; }
    [JsonProperty]
    public int Y { get; private set; }
    [JsonProperty]
    public int Z { get; private set; }

    public SaveFindItem() {}
    public SaveFindItem(int code, Vector3Int pos) {
        Code = code;
        X = pos.x;
        Y = pos.y;
        Z = pos.z;
    }
}
public class SaveFindEvent {
    [JsonProperty]
    public int Code { get; private set; }
    [JsonProperty]
    public int X { get; private set; }
    [JsonProperty]
    public int Y { get; private set; }
    [JsonProperty]
    public int Z { get; private set; }

    public SaveFindEvent() {}

    public SaveFindEvent(int code, Vector3Int pos) {
        Code = code;
        X = pos.x;
        Y = pos.y;
        Z = pos.z;
        
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
        Skill = playerInfo.Skill.ToArray();
        HaveSkill = playerInfo.Skill.ToArray();
        MaximumHp = playerInfo.MaximumHp;
        CurrentHp = playerInfo.CurrentHp;
        MaximumMp = playerInfo.MaximumMp;
        CurrentMp = playerInfo.CurrentMp;
    }
}

public class SaveUseItem {
    [JsonProperty]
    public int Code { get; private set; }
    [JsonProperty]
    public int Count { get; private set; }

    public SaveUseItem() { }

    public SaveUseItem(int code, int count) {
        Code = code;
        Count = count;
    }
}

public class SaveData {

    public static void Load(int index = 0) {
        string rawData;
        if(index == 0)
            rawData = File.ReadAllText(Application.streamingAssetsPath + @"/SaveFile/DefaultSaveData.json");
        else 
            rawData = File.ReadAllText(Application.streamingAssetsPath + $@"/SaveFile/SaveData{index}.json");
        var data = JsonConvert.DeserializeObject<SaveFormat>(rawData);
        
        foreach (var character in data.SaveCharacterInfo) {

            //characterInfo Load
            var before = GameObject.Find($"{character.Name}Info");
            if(before is not null) 
                GameObject.Destroy(before);
            
            var target = new GameObject();
            target.name = $"{character.Name}Info";
            var info = target.AddComponent<EachCharacterInfo>();
            info.Load(character);
        }
        
        CurrentMapInfo.SetData(data.SavePos.Code, new Vector2(data.SavePos.X, data.SavePos.Y));
        PlayTime.Load(data.PlayTime);
        ChapterInfo.Set(data.SavePos.Chapter);
        Inventory.Load(data.SaveItem,data.SaveUseItem);
        AlreadyFindEventInfo.Load(data.SaveFindItem, data.SaveFindEvent, data.SaveEvent);
        MonsterInfo.Load(data.SaveMonster);
    }
    
    public static void Save(int saveSlot, EachCharacterInfo[] playerInfos, int chapter, int mapCode, Vector2 pos) {
 
        var json = new JObject();
        json.Add("PlayTime", PlayTime.Save());
        SerializeCharacter(json, playerInfos);
        SerializeInventory(json);
        SerializeUseItem(json);
        SerializePos(json, chapter, mapCode, pos);
        SerializeMonster(json);
        SerializeFindEvent(json);
        SerializeFindItem(json);
        SerializeEvent(json);
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

    private static void SerializePos(JObject json, int chapter, int code, Vector2 pos) {
        var fixInfos = new SavePos(code, pos, chapter);
        JObject parse = JObject.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SavePos), parse);
    }

    private static void SerializeMonster(JObject json) {
        var fixInfos = MonsterInfo.Monsters.Select(monster => new SaveMonster(monster.Key, monster.Value));
        var parse = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SaveMonster), parse);
    }
    
    private static void SerializeFindItem(JObject json) {
        var fixInfos = AlreadyFindEventInfo.FindItemInfos
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
        var fixInfos = AlreadyFindEventInfo.FindEventInfos
            .Select(findEvent => {
                var events = findEvent.Value
                    .Select(factor => new SaveFindEvent(findEvent.Key, factor));
                return events.ToList();
            })
            .SelectMany(factor => factor);
        var parse = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SaveFindEvent), parse);
    }

    private static void SerializeEvent(JObject json) {
        var fixInfos = AlreadyFindEventInfo.Events.ToList();
        var parse = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add("SaveEvent", parse);
    }

    private static void SerializeUseItem(JObject json) {
        var fixInfos = Inventory.Use
            .Select(factor => new SaveUseItem(factor.Key, factor.Value));
        var parse = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SaveUseItem), parse);
    }
}