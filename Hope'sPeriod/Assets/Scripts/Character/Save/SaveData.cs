using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class SaveData {

    public class SaveItem {
        public int Code { get; private set; }
        public int Amount { get; private set; }

        public SaveItem(int code, int amount) {
            Code = code;
            Amount = amount;
        }
    }
    public class SaveMonster {
        public int Code { get; private set; }
        public int KillCount { get; private set; }

        public SaveMonster(int code, int killCount) {
            Code = code;
            KillCount = killCount;
        }
    }
    public class SavePos {
        public int Code { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public SavePos(int code, Vector2Int pos) {
            Code = code;
            X = pos.x;
            Y = pos.y;
        }
    }
    public class SaveFindItem {
        public int Code { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public SaveFindItem(int code, Vector2Int pos) {
            Code = code;
            X = pos.x;
            Y = pos.y;
        }
    }
    public class SaveFindEvent {
        public int Code { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public SaveFindEvent(int code, Vector2Int pos) {
            Code = code;
            X = pos.x;
            Y = pos.y;
        }
    }
    public class SavePlayerInfo {
        
        public string Name { get; private set; }
        public bool Exist;
        public bool Dead;
        public int[] Skill { get; private set; }
        public int[] HaveSkill { get; private set; }
        public float MaximumHp { get; private set; }
        public float CurrentHp { get; private set; }
        public float MaximumMp { get; private set; }
        public float CurrentMp { get; private set; }

        public SavePlayerInfo(EachCharacterInfo playerInfo) {

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
    
    public static void Save(EachCharacterInfo[] playerInfos, int mapCode, Vector2Int pos, int saveSlot) {
 
        var json = new JObject();
        SerializeCharacter(json, playerInfos);
        SerializeInventory(json);
        SerializePos(json, mapCode, pos);
        SerializeMonster(json);
        SerializeFindEvent(json);
        SerializeFindItem(json);
        File.WriteAllText(Application.streamingAssetsPath + $@"/SaveFile/SaveFile{saveSlot}.json", json.ToString(Formatting.Indented));
    }

    private static void SerializeCharacter(JObject json, EachCharacterInfo[] playerInfos) {
        var fixInfos = playerInfos.Select(info => new SavePlayerInfo(info));
        var jPlayerInfo = JArray.Parse(JsonConvert.SerializeObject(fixInfos, Formatting.Indented));
        json.Add(nameof(SavePlayerInfo), jPlayerInfo);
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