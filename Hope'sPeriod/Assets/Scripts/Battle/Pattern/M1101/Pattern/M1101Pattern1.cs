using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

//sin attack
public class M1101Pattern1: EnemyPatternBase {
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject hWarnning;
    [SerializeField] private GameObject vWarnning;
    private SpriteRenderer hRenderer;
    private SpriteRenderer vRenderer;
    
    public override bool Active { get; protected set; }
    
    private const float WidthLength = 6f;
    private const float HeightLength = 3.5f;
    
    private IEnumerator Pattern(float sinDelay, float sinCount, float amplification, float speed, float cycle, Action callback = null) {

        Active = true;
        hRenderer.DOBlink(0.3f, 0.1f, 0.3f)
            .OnComplete(() => hRenderer.DOBlink(0.3f, 0.1f, 0.3f));
        vRenderer.DOBlink(0.3f, 0.1f, 0.3f)
            .OnComplete(() => vRenderer.DOBlink(0.3f, 0.1f, 0.3f));
        yield return new WaitForSeconds(1.5f);
        
        bool direction = Random.Range(0,1f) > 0.5 ? true : false;
        float x;
        float y;
        
        x = (direction ? -1 : 1) * WidthLength;
        y = BaseEnemy.Player.transform.localPosition.y;
        var hPosition = new Vector3(x, y, -1);
        
        x = BaseEnemy.Player.transform.localPosition.x;
        y = (direction ? -1 : 1) * HeightLength;
        var vPosition = new Vector3(x, y, -1);
        
        var hTargetPos = hPosition;
        var vTargetPos = vPosition;
        hTargetPos.x *= -1;
        vTargetPos.y *= -1;
            
        while (sinCount-- != 0) {

            yield return new WaitForSeconds(sinDelay);
            Make(hPosition, hTargetPos);
            Make(vPosition, vTargetPos);
        }
        
        yield return new WaitForSeconds(1);
        callback?.Invoke();
        yield break;

        void Make(Vector3 start, Vector3 end) {
            var newObject = Instantiate(bullet);
                        
            newObject.transform.localPosition = start;
            
            
            var @object = newObject.GetComponent<M1101Object1>();
            @object.SetPos(end);
            @object.SetAmplificationPower(amplification);
            @object.SetSpeed(speed);
            @object.SetCycle(cycle);
        }
    }

    private int count = 3;
    private float delay = 3f; 
    public override void StartPattern() {
        MapSizeManager.Instance.Default();

        hRenderer = hWarnning.GetComponent<SpriteRenderer>();
        vRenderer = vWarnning.GetComponent<SpriteRenderer>();

        float time = 0;
        Action pattern = () =>  
            StartCoroutine(Pattern(0.2f, 15, 4.5f, 6, 2));
        
        for(int i = 0; i < count; i++) {
            if(i + 1 == count)
                pattern = () =>  
                    StartCoroutine(Pattern(0.2f, 15, 4.5f, 6, 2, () => Active = false));
            StartCoroutine(Wait.WaitAndDo(time, pattern));
            time += delay;
        }
    }

    public void Update() {
        var pos = BaseEnemy.Player.transform.localPosition;
        vWarnning.transform.localPosition = new(pos.x, -0.46f);
        hWarnning.transform.localPosition = new(0, pos.y);
    }

    public override void GrouptPattern() {
    }
}