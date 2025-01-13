using UnityEngine;

[CreateAssetMenu(menuName ="Item")]
public class ItemFormat : ScriptableObject {

    [SerializeField] private int code;
    public int Code => code;
    [SerializeField] private string itemName;
    public string ItemName => itemName;
    [SerializeField] private string description;
    public string Description => description;
    [SerializeField] private int healHp;
    public int HealHp => healHp;
    [SerializeField] private int healsHp;
    public int HealsHp => healsHp;
    [SerializeField] private int healMp;
    public int HealMp => healMp;
    [SerializeField] private int healsMp;
    public int HealsMp => healsMp;
}