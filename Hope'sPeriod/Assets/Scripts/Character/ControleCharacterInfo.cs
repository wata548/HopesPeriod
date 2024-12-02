public class ControleCharacterInfo {

    public static ControleCharacterInfo Instance { get; private set; } = null;
    public 
    void SetSingleton() {

        if (Instance == null) {
            Instance = this;
        }
    }

    private void Awake() {

        SetSingleton();
    }
    
}