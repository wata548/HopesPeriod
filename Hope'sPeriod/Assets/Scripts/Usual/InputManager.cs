using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using System.Reflection;

public enum KeyTypes { 

    UP,
    DOWN,
    LEFT,
    RIGHT,
    JUMP,
    SELECT,
    INTERACTION
}

public class KeyState {

    [JsonProperty]
    private readonly KeyCode key;

    [JsonProperty]
    private readonly float REQUIRED_HOLD_TIME;

    private bool    isClick = false;

    private bool    isHold = false;
    private float   currentHoldTime = 0;

    private bool    isUp = false;

    public void StateUpdate() {

        isClick = false;
        isUp = false;

        if(Input.GetKeyDown(key)) {

            isClick = true;
            isHold = true;
        }

        else if(Input.GetKey(key)) {

            if(currentHoldTime < REQUIRED_HOLD_TIME) {

                currentHoldTime += Time.deltaTime;
            }
        }

        else if(Input.GetKeyUp(key)) {

            isHold = false;
            currentHoldTime = 0;
            isUp = true;
        }
    }

    public bool Click() => isClick;
    public bool Pressing() => isHold;
    public bool Hold() => (currentHoldTime >= REQUIRED_HOLD_TIME);
    public bool Up() => isUp;
    

    public KeyState(KeyCode key, float requiredTime = 0.5f) {
        
        this.key = key;
        REQUIRED_HOLD_TIME = requiredTime;
    }
}

public class InputManager : MonoBehaviour
{

//==================================================| Field 

    public static InputManager Instance { get; private set; } = null;

    public Dictionary<KeyTypes, KeyState> KeyMapper { get; private set; } = new();

//==================================================| Method

    #region CheckState

    private bool ExsistCheck(KeyTypes type) {

        return KeyMapper.ContainsKey(type);
    }

    public bool Click(KeyTypes type) {

        if (!ExsistCheck(type)) {

            return false;
        }

        return KeyMapper[type].Click();

    }

    public bool Hold(KeyTypes type) {

        if (!ExsistCheck(type)) {

            return false;
        }

        return KeyMapper[type].Hold();

    }

    public bool Up(KeyTypes type) {

        if (!ExsistCheck(type)) {

            return false;
        }

        return KeyMapper[type].Up();

    }

    public bool Pressing(KeyTypes type) {

        if (!ExsistCheck(type)) {

            return false;
        }

        return KeyMapper[type].Pressing();

    }

    #endregion

    #region ConvertJson

    public void SerializeJson(string fileName) {

        if (fileName == "Default" || fileName == "DefaultArrow") {

            Debug.Log("You cann't use this name");

            return;
        }

        string path = $"Assets\\Resources\\KeyBindSetting\\{fileName}.json";

        JsonSerializerSettings settings = new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };

        string json;
        json = JsonConvert.SerializeObject(KeyMapper, settings);

        File.WriteAllText(path, json);

        Debug.Log(json);
    }

    // Change Key Mapping
    public void KeySettingLoad(Dictionary<KeyTypes, KeyState> newKeyMapper) {

        KeyMapper = newKeyMapper;
    }

    public Dictionary<KeyTypes, KeyState> DeserializeJson(string fileName) {

        string path = $"Assets\\Resources\\KeyBindSetting\\{fileName}.json";

        JsonSerializerSettings settings = new JsonSerializerSettings {
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };

        if (!File.Exists(path)) {

            throw new Exception("This file is not exsist. Please check file name");
        }

        string json = File.ReadAllText(path);

        var newSetting = JsonConvert.DeserializeObject<Dictionary<KeyTypes, KeyState>>(json, settings);
        return newSetting;
    }

    #endregion

//==================================================| Unity Logic

    private void Awake() {
        
        // Singleton pattern
        if(Instance == null) {

            Instance = this;
        }

        DeserializeJson("DefaultArrow");
    }

    void Update()
    {
        //* Update each Key's state
        foreach(KeyState state in KeyMapper.Values) {

            state.StateUpdate();
        }

        //* testCode
        //Debug.Log($"Click: {Click(KeyTypes.UP)}, Pressing: {Pressing(KeyTypes.UP)}, Hold: {Hold(KeyTypes.UP)}, UP: {Up(KeyTypes.UP)}");
    }
}
