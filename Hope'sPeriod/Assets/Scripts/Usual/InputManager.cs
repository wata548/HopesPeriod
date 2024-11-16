using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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

    private readonly KeyCode key;
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
    public bool Hold() => (currentHoldTime >= REQUIRED_HOLD_TIME);
    public bool Up() => isUp;
    

    public KeyState(KeyCode key, float requiredTime = 0.5f) {
        
        this.key = key;
        REQUIRED_HOLD_TIME = requiredTime;
    }
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; } = null;

    public Dictionary<KeyTypes, KeyState> KeyMapper { get; private set; } = new();

    private void Awake() {
        
        if(Instance == null) {

            Instance = this;
        }
    }

    void Update()
    {
        //* Update each Key's state
        foreach(KeyState state in KeyMapper.Values) {

            state.StateUpdate();
        }

    }

    //* Change Key Mapping
    public void NewKeySetting(Dictionary<KeyTypes, KeyState> newKeyMapper) {

        KeyMapper = newKeyMapper;
    }

    //* Check State
    public bool Click(KeyTypes type) {
        
        if(!KeyMapper.ContainsKey(type)) {

            return false;
        }

        return KeyMapper[type].Click();

    }

    public bool Hold(KeyTypes type) {

        if (!KeyMapper.ContainsKey(type)) {

            return false;
        }

        return KeyMapper[type].Hold();

    }

    public bool Up(KeyTypes type) {

        if (!KeyMapper.ContainsKey(type)) {

            return false;
        }

        return KeyMapper[type].Up();

    }
}
