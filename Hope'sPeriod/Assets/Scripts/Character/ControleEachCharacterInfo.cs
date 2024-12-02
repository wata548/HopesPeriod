public class ControleEachCharacterInfo: ICharacterInfo {
    
    public float MaximumHp { get; private set; }
    public float CurrentHp { get; private set; }
    
    public float MaximumMp { get; private set; }
    public float CurrentMp { get; private set; }

    public bool UseableMp(float power) =>CurrentMp >= power;

    public bool UseMp(float power) {

        if (!UseableMp(power)) {
            return false;
        }

        CurrentMp -= power;
        return true;
    }
    
    public bool GetDamageable(float damage) => CurrentHp > damage;

    public bool GetDamage(float damage) {

        if (!GetDamageable(damage)) {
            return false;
        }

        CurrentHp -= damage;
        return true;
    }
}