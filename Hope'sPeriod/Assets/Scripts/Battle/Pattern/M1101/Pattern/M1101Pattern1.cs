using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

//sin attack
public class M1101Pattern1: EnemyPatternBase {
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject hWarnning;
    [SerializeField] private GameObject vWarnning;
    [SerializeField] private List<Sprite> horizon;
    [SerializeField] private List<Sprite> vertical;
    
    private SpriteRenderer hRenderer;
    private SpriteRenderer vRenderer;
    
    public override bool Active { get; protected set; }
    
    private const float WidthLength = 6f;
    private const float HeightLength = 3.5f;
    
    private IEnumerator Pattern(float sinDelay, float sinCount, float amplification, float speed, float cycle, Action callback = null) {

        hRenderer.DOBlink(0.3f, 0.1f, 0.3f)
            .OnComplete(() => hRenderer.DOBlink(0.3f, 0.1f, 0.3f, 0.8f));
        vRenderer.DOBlink(0.3f, 0.1f, 0.3f)
            .OnComplete(() => vRenderer.DOBlink(0.3f, 0.1f, 0.3f, 0.8f));
        yield return new WaitForSeconds(1.5f);
        
        float x;
        float y;
        
        x = (BaseEnemy.Player.transform.localPosition.x > 0 ? -1 : 1) * WidthLength;
        y = BaseEnemy.Player.transform.localPosition.y;
        var hPosition = new Vector3(x, y, -1);
        
        x = BaseEnemy.Player.transform.localPosition.x;
        y = (BaseEnemy.Player.transform.localPosition.y > 0 ? -1 : 1) * HeightLength;
        var vPosition = new Vector3(x, y, -1);
        
        var hTargetPos = hPosition;
        var vTargetPos = vPosition;
        hTargetPos.x *= -1;
        vTargetPos.y *= -1;
            
        while (sinCount-- != 0) {

            yield return new WaitForSeconds(sinDelay);
            Make(hPosition, hTargetPos, false);
            Make(vPosition, vTargetPos, true);
        }
        
        yield return new WaitForSeconds(3);
        callback?.Invoke();
        yield break;

        void Make(Vector3 start, Vector3 end, bool isVertical) {
            var newObject = Instantiate(bullet);
                        
            newObject.transform.localPosition = start;

            if (isVertical) {
                int index = Random.Range(0, vertical.Count);
                newObject.GetComponent<SpriteRenderer>().sprite = vertical[index];
                if(start.y > 0)
                    newObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else {
                int index = Random.Range(0, horizon.Count);
                var renderer = newObject.GetComponent<SpriteRenderer>();
                renderer.sprite = horizon[index];
                renderer.flipX = start.x > 0;
            }
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
        
        Active = true;
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
            time += delay + 0.35f;
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