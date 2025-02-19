using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterInfoInventory: MonoBehaviour {

    [field: SerializeField] public List<EachChararacterInfoInventory> Characterinfos { get; private set; }
    public static CharacterInfoInventory Instance { get; private set; } = null;
    public EachChararacterInfoInventory GetCharacterInfo(int index) => Characterinfos[index];
    public List<EachCharacterInfo> RawCharacterInfos => Characterinfos.Select(info => info.Info).ToList();
    
    private void Awake() {
        Instance = this;
    }

    public void UpdateState() {
        foreach (var character in Characterinfos) {
            character.UpdateState();
        }
    }
}