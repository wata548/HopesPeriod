using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

public class M1101Pattern1: EnemyPatternBase {
    [SerializeField] private GameObject bullet;
    
    public override bool Active { get; protected set; }
    
    private const float WidthLength = 6f;
    private const float HeightLength = 3.5f;
    
    private IEnumerator Pattern(float sinDelay, float sinCount, float amplification, float speed, float cycle, Action callback = null) {

        Active = true;

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

    private int count = 2;
    private float delay = 2.5f; 
    public override void StartPattern() {
        MapSizeManager.Instance.Default();

        float time = 0;
        Action pattern = () =>  
            StartCoroutine(Pattern(0.2f, 13, 4.5f, 6, 2));
        
        for(int i = 0; i < count; i++) {
            if(i + 1 == count)
                pattern = () =>  
                    StartCoroutine(Pattern(0.2f, 13, 4.5f, 6, 2, () => Active = false));
            StartCoroutine(Wait.WaitAndDo(time, pattern));
            time += delay;
        }
    }

    public override void GrouptPattern() {
    }
}