using System;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class LoadButton: InteractButton {

    public bool On { get; private set; } = false;
    public static MapEventInfo mapInfo = null;
    public class SimpleSaveInfo {
        [JsonProperty]
        public float PlayTime { get; private set; }
        [JsonProperty]
        public SavePos SavePos { get; private set; }
    }

    private TMP_Text text;
    public void SetUp() {
        if (mapInfo is null)
            mapInfo = Resources.Load<MapEventInfo>("MapPrefab/MapInfo");
        
        text = GetComponentInChildren<TMP_Text>();
        if (Index == 0) {
            On = true;
            return;
        }
        
        var rawData = File.ReadAllText(Application.streamingAssetsPath + $@"\SaveFile\SaveData{Index}.json");
        var info = JsonConvert.DeserializeObject<SimpleSaveInfo>(rawData);

        if (info.SavePos.Chapter == 0) {
            On = false;
            text.text = "\t " + "빈 슬롯".SetSize(1.2f).AddColor(Color.gray);
            return;
        }
        
        On = true;
        string hour = $"{(int)info.PlayTime / 60 / 60}".PadLeft(2, '0');
        string minute = $"{(int)info.PlayTime / 60 % 60}".PadLeft(2, '0');
        string second = $"{(int)info.PlayTime % 60}".PadLeft(2, '0');
        string time = $"{hour}:{minute}:{second}".AddColor(Color.gray).SetSize(0.6f);

        text.text =
            $"{$"{Index}|".SetSize(1.6f)} {info.SavePos.Chapter}장 - {mapInfo.Name(info.SavePos.Code).SetSize(0.8f)} {time}";
    }

    public void SetColor(Color color) {
        text.color = color;
    } 
    
    public override void Click() {}

    public void Load() {
        if (!On)
            return;
        SaveData.Load(Index);
        ScenceControler.Load("PlayMap");
    }
    
    public void Save() {
        SaveData.Save(Index, CharacterInfoInventory.Instance.RawCharacterInfos.ToArray() ,ChapterInfo.Chapter, CheckEvent.MapCode, TilePlayerPhysics.Pos );
        SetUp();
    }
}