using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

//Many needle drop
//Time: 10(0.6 + 1) + 1 = 17
public class M1001Pattern3: EnemyPatternBase{

    public override bool End { get; protected set; } = false;
    [SerializeField] private GameObject prefab;
    [SerializeField] private SpriteRenderer[] warning = new SpriteRenderer[3];
    [SerializeField] private List<Sprite> images = new();
    private const float WidthCount = 4;
    private const float HeightCount = 6;
    private readonly Vector3 defaultPosition = new(-2.5f, 3.5f, -1);
    
    
    private const float Density = 0.1f;
    IEnumerator WaitAndSpawn(float time, int count) {

        End = true;
        for (int i = 0; i < count; i++) {

            var pos = defaultPosition;
            int part = Random.Range(-1, 1 + 1);

            pos.x += part * WidthCount;

            warning[part + 1].DOBlink(0.2f, 0.2f, 0.2f, 0.4f)
                .OnComplete(() => warning[part + 1].DOBlink(0.2f, 0.2f, 0.2f, 0.4f));
            
            yield return new WaitForSeconds(time);
            for (int j = 0; j < HeightCount; j++) {
                
                yield return new WaitForSeconds(Density);
                for (int k = 0; k < WidthCount; k++) {
                    var newObject = Instantiate(prefab);
                    newObject.transform.rotation = quaternion.Euler(0, 0, Random.Range(-60, 60));
                    newObject.GetComponent<SpriteRenderer>().sprite = images[Random.Range(0, images.Count)];
                    newObject.AddComponent<M1001Object3>();

                    pos.x++;
                    newObject.transform.position = pos;
                }
                pos.x -= WidthCount;    
            }


        }

        yield return new WaitForSeconds(1);
        End = false;
    }

    public override void StartPattern() {

        MapSizeManager.Instance.Default();
        StartCoroutine(WaitAndSpawn(1, 10));
    }

};