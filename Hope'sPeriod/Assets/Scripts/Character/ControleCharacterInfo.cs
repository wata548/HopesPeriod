using UnityEngine;

public class ControleCharacterInfo: MonoBehaviour {
    
    
   //==================================================| Field 
    public static ControleCharacterInfo Instance { get; private set; } = null;
    
    private int characterCount = 0;
    private const int MAXIMUM_CHARCTER_COUNT = 3;
    private ControleEachCharacterInfo[] characterinfos = new ControleEachCharacterInfo[3];
    private const float INVINCIBILITY_TIME = 1.2f;
    #region Singleton 
    void SetSingleton() {
        
        if (Instance == null) {
            Instance = this;
        }
    }
    
    #endregion
    
   //==================================================| Method 
    public bool AddCharcter(ControleEachCharacterInfo character) {
        
        if (characterCount >= MAXIMUM_CHARCTER_COUNT) {

            return false;
        }

        characterinfos[characterCount++] = character;
        return true;
    }

    public bool ChangePosition(int index1, int index2) {

        if (Mathf.Max(index1, index2) >= characterCount) {
            
            return false;
        }

        (characterinfos[index1], characterinfos[index2]) = (characterinfos[index2], characterinfos[index1]);
        return true;
    }
    
   //=================================================| UnityLogic 
    private void Awake() {
    
        SetSingleton();
    }
}