using TMPro;
using UnityEngine;

public class ShowInvincibility : MonoBehaviour {
    private TMP_Text text;
    
    void Awake() {
        text = GetComponent<TMP_Text>();
    }

    void Update() {

        text.text = $"{Invincibility.Instance.InvincibilityTime}: {Invincibility.Instance.CurrentTime}";
    }
}
