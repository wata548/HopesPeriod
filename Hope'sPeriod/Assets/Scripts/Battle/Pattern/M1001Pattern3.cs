using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class M1001Pattern3: EnemyPatternBase{

    public override bool End { get; protected set; } = false;
    [SerializeField] private GameObject prefab;
    [SerializeField] private SpriteRenderer[] warning = new SpriteRenderer[3];
    private const float startMove = 1.5f;
    private const float widthCount = 4;
    private const float heightCount = 6;
    
    IEnumerator WaitAndSpawn(float time, int count) {

        End = true;
        for (int i = 0; i < count; i++) {

            var pos = new Vector3(-2.5f, 3.5f, -1);
            int part = Random.Range(-1, 1 + 1);

            pos.x += part * widthCount;

            warning[part + 1].DOBlink(0.2f, 0.2f, 0.2f, 0.4f)
                .OnComplete(() => warning[part + 1].DOBlink(0.2f, 0.2f, 0.2f, 0.4f));
            
            yield return new WaitForSeconds(1);
            for (int j = 0; j < heightCount; j++) {
                
                yield return new WaitForSeconds(0.1f);
                for (int k = 0; k < widthCount; k++) {
                    var newObject = Instantiate(prefab);
                    newObject.AddComponent<M1001Object3>();

                    pos.x++;
                    newObject.transform.position = pos;
                }
                pos.x -= widthCount;    
            }


        }

        End = false;
    }

    public override void StartPattern() {

        StartCoroutine(WaitAndSpawn(0.5f, 100));
    }

};