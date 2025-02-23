using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

//Trace bullet zen
//Time: 0.5 * 15 + 3 = 10.5
public class M1001Pattern2: EnemyPatternBase{

    public override bool End { get; protected set; } = false;
    [SerializeField] private GameObject prefab;
    private readonly Vector2 fieldRange = new Vector2(8.5f, 4.5f);
    private readonly Vector2 safeZoneRange = new Vector2(5.5f, 3f);
    private const float startMove = 1.5f;
    [SerializeField] private List<Sprite> images = new();
    
    IEnumerator WaitAndSpawn(float time, int count) {

        End = true;
        for (int i = 0; i < count; i++) {

            yield return new WaitForSeconds(time);

            var pos = new Vector3(Random.Range(-fieldRange.x, fieldRange.x), Random.Range(-fieldRange.y, fieldRange.y), -1);
            if (-safeZoneRange.x <= pos.x && pos.x <= safeZoneRange.x || -safeZoneRange.y <= pos.y && pos.y <= safeZoneRange.y) {
                while (-safeZoneRange.x <= pos.x && pos.x <= safeZoneRange.x) {
                    pos.x = Random.Range(-fieldRange.x, fieldRange.x);
                }
            }

            var newObject = Instantiate(prefab);
            newObject.GetComponent<SpriteRenderer>().sprite = images[Random.Range(0, images.Count)];
            StartCoroutine(Wait.WaitAndDo(startMove, () => newObject.AddComponent<M1001Object2>()));
            newObject.transform.position = pos;

        }
        yield return new WaitForSeconds(startMove * 2);

        End = false;
    }

    public override void StartPattern() {

        MapSizeManager.Instance.Default();
        StartCoroutine(WaitAndSpawn(0.5f, 15));
    }
};