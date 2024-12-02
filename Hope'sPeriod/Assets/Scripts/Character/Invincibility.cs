using UnityEngine;

public class Invincibility {

   //==================================================| Field 
    public static Invincibility Instance { get; private set; } = null;
    private float invincibilityTime = 1.2f;
    private float currentTime = 0;
    private bool invincibility = false;

    //==================================================| Method
    public void Init(float invincibilityTime = 1.2f) {
        this.invincibilityTime = invincibilityTime;
        currentTime = 0;
        invincibility = false;
    }
    public void TurnOn() {
        invincibility = true;
        currentTime = 0;
    } 
    private void SetSingleton() {

        if (Instance == null) {
            Instance = this;
        }
    }
    
    private bool UpdateTime() {

        if (!invincibility) {
            return false;
        }

        currentTime += Time.deltaTime;
        if (currentTime >= invincibilityTime) {
            invincibility = false;
        }
        return invincibility;
    }

   //==================================================| UnityLogic 
    private void Awake() {

        SetSingleton();
    }
    private void Update() {

        UpdateTime();
    }
}