using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapChanger: MonoBehaviour {

    [SerializeField] private TMP_Text mapName;
    [SerializeField] private GameObject box;
    
    [SerializeField] private float resultPos;
    [SerializeField] private float startPos;
    [SerializeField] private float appearTime;
    [SerializeField] private float stayTime;
    private Tween doing = null;
    public void Show(string name) {

        mapName.text = name;

        if (doing is not null)
            doing.Kill();

        var image = box.GetComponent<Image>();
        var pos = box.transform.localPosition;
        image.DOFade(1, 0);
        mapName.DOFade(1, 0);
        
        pos.x = startPos;
        box.transform.localPosition = pos; 
        Sequence animation = DOTween.Sequence();
        animation.Append(box.transform.DOLocalMoveX(resultPos, appearTime).SetEase(Ease.OutBack));
        animation.AppendInterval(stayTime);
        animation.Append(image.DOFade(0, appearTime));
        animation.Append(mapName.DOFade(0, appearTime));

        doing = animation;
    }
}