using System;
using SpreadInfo;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowEffect: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private SpriteRenderer image;
    [SerializeField] private TMP_Text duration;

    private Type showType;
    private IEffect effect;

    public void Destroy() {
        Destroy(gameObject);
    }

    public string Info() {
        string result = "";
        if (effect is ShieldInfo shield) {

            if (shield.Reflect) {
                
                result += shield.Type switch {
                    DefenceType.Time =>
                        $"{(int)shield.Duration}턴 동안 ",
                    DefenceType.Break => $"{(int)shield.Duration}만큼 "
                };   
                result += "자신이 받는 데미지를 적이 대신 받습니다.";
            }
            else {
                
                result = shield.Type switch {
                    DefenceType.Time =>
                        $"{(int)shield.Duration}턴 동안 {(int)((shield.Duration - (int)shield.Duration) * 100)}%의 데미지가 경감됩니다.",
                    DefenceType.Break => $"{(int)shield.Duration}만큼 방패가 대신 데미지를 입습니다."
                };
            }
        }

        if (effect is AttractInfo attract) {
            result = $"{(int)attract.Duration}턴 동안 전체 데미지의 {(int)(attract.Power * 100)}%를 대신 받습니다. (가장 높은 수치의 도발만 적용됩니다.)";
        }

        if (effect is EffectInfo effectinfo) {
            if (effectinfo.Type == EffectType.AttackUp)
                result = $"{(int)effectinfo.Duration}턴 동안 공격력이 {(int)(effectinfo.Power * 100)}%만큼 상승합니다.";
        }

        return result;
    }
    
    public void Set(AttractInfo target)  {

        showType = typeof(AttractInfo);
        effect = target;

        string imageName = "EffectImage/";
        imageName += "Attract";
        
        image.sprite = Resources.Load<Sprite>(imageName);
        Duration();
    }

    public void Set(ShieldInfo target) {
        showType = typeof(ShieldInfo);
        effect = target;
        
        string imageName = "EffectImage/";
        if (target.Reflect)
            imageName += "Absolute";
        imageName += "Shield";
        image.sprite = Resources.Load<Sprite>(imageName);
        Duration();
    }
    
    public void Set(EffectInfo target) {
        showType = typeof(ShieldInfo);
        effect = target;
            
        string imageName = "EffectImage/";
        imageName += target.Type.ToString();
    
        image.sprite = Resources.Load<Sprite>(imageName);
        Duration();
    }

    private void Duration() {
        duration.text = ((int)effect.Duration).ToString();
    }

    private bool on = false;
    public void OnPointerEnter(PointerEventData eventData) {

        ShowEffectInfo.Instance.TurnOn();
        on = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        ShowEffectInfo.Instance.TurnOff();
        on = false;
    }

    private void Update() {
        
        if (on)
            ShowEffectInfo.Instance.SetData(Input.mousePosition, Info());
    }
}