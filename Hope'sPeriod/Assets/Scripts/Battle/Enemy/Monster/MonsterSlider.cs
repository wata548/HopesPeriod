using TMPro;
using UnityEngine;

public class MonsterSlider: Slider {

    [SerializeField] private GameObject shower;
    private TMP_Text showerText;
    private SpriteRenderer showerRenderer;
    
    private int maxHp;
    private int currentHp;
    public bool Dead { get; private set; } = false;

    private readonly Color heal = Color.green;
    private readonly Color damaged = Color.red;
    
    public void SetMonster() {

        maxHp = Monster.Instance.MaxHP;
        currentHp = maxHp;
    }

    public void Damaged(int power) {

        showerText.text = power.ToString();
        showerText.color = damaged;
        
        if (power >= currentHp) {
            power = 0;
            currentHp = 0;
            Dead = true;
            Debug.Log("win");
            //TODO: Reward system and wining event 
        }
        else {
            currentHp -= power;
        }

        UpdateInfo(currentHp / maxHp);
        showerRenderer.DOBlink(0.2f, 0.2f, 0.2f);
        showerText.DOBlink(0.2f, 0.2f, 0.2f);
    }

    public void Heal(int power) {

        showerText.text = power.ToString();
        showerText.color = heal;

        currentHp += power;
        currentHp = Mathf.Min(currentHp, maxHp);

        UpdateInfo(currentHp / maxHp);
        showerText.DOBlink(0.2f, 0.2f, 0.2f);
    }

    private void Awake() {
        showerRenderer = shower.GetComponent<SpriteRenderer>();
        showerText = shower.GetComponentInChildren<TMP_Text>();
    }
}