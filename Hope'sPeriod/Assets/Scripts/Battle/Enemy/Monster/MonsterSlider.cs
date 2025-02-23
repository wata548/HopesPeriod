using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MonsterSlider: Slider {

    [SerializeField] private GameObject shower;
    private TMP_Text showerText;
    private SpriteRenderer showerRenderer;
    
    public int MaxHp { get; private set; }
    public int CurrentHp { get; private set; }
    public bool Dead { get; private set; } = false;
    public static MonsterSlider Instance { get; private set; } = null;
    
    private readonly Color heal = Color.green;
    private readonly Color damaged = Color.red;
    
    public void SetMonster() {

        MaxHp = Monster.Instance.MaxHP;
        CurrentHp = MaxHp;
    }

    public void GetDamage(int power) {

        showerText.text = power.ToString();
        showerText.color = damaged;
        
        if (power >= CurrentHp) {
            power = 0;
            CurrentHp = 0;
            Dead = true;
            Debug.Log("win");

            MonsterInfo.Kill();
            GameFSM.Instance.Win();
        }
        else {
            CurrentHp -= power;
        }

        UpdateInfo((float)CurrentHp / MaxHp);
        ShakeCamera.Instance.Shake(0.4f, 0.3f).DOBeforeWait(0.2f);
        showerRenderer.DOBlink(0.2f, 0.4f, 0.2f);
        showerText.DOBlink(0.2f, 0.4f, 0.2f);
    }

    public void Heal(int power) {

        showerText.text = power.ToString();
        showerText.color = heal;

        CurrentHp += power;
        CurrentHp = Mathf.Min(CurrentHp, MaxHp);

        UpdateInfo(CurrentHp / MaxHp);
        showerText.DOBlink(0.2f, 0.2f, 0.2f);
    }

    private void Awake() {

        MonsterInfo.MakeMonster();
        
        base.Awake();
        
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
        
        showerRenderer = shower.GetComponent<SpriteRenderer>();
        showerText = shower.GetComponentInChildren<TMP_Text>();
        SetMonster();
    }
}