using UnityEngine;

public class Invincibility : MonoBehaviour {

   //==================================================| Field 
    public static Invincibility Instance { get; private set; } = null;
    public float CurrentTime { get; private set; } = 0;
    public float InvincibilityTime { get; private set; }= 1.2f;
    public bool OnInvincibility { get; private set; } = false; 
    
    //==================================================| Method
    public void Init(float invincibilityTime = 1.2f) {
        this.InvincibilityTime = invincibilityTime;
        CurrentTime = 0;
        OnInvincibility = false;
    }
    public bool TurnOn() {

        if (OnInvincibility)
            return false;
        
        OnInvincibility = true;
        CurrentTime = 0;

        return true;
    } 
    private void SetSingleton() {

        if (Instance == null) {
            Instance = this;
        }
    }
    
    private bool UpdateTime() {

        if (!OnInvincibility) {
            return false;
        }

        CurrentTime += Time.deltaTime;
        if (CurrentTime >= InvincibilityTime) {
            OnInvincibility = false;
        }
        return OnInvincibility;
    }

   //==================================================| UnityLogic 
    private void Awake() {

        SetSingleton();
    }
    private void Update() {

        UpdateTime();
    }
}