using System;
using System.CodeDom;
using SpreadInfo;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShowEffect: MonoBehaviour {

    [SerializeField] private SpriteRenderer image;
    [SerializeField] private TMP_Text duration;

    private Type showType;
    private IEffect effect;

    public void Destroy() {
        Destroy(gameObject);
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
    
}