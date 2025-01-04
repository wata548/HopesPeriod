using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

//Trace bullet zen
public class M1001Pattern2: EnemyPatternBase{

    public override bool End { get; protected set; } = false;
    [SerializeField] private GameObject prefab;
    private readonly Vector2 fieldRange = new Vector2(8.5f, 4.5f);
    private readonly Vector2 safeZoneRange = new Vector2(5.5f, 3f);
    private const float startMove = 1.5f;
    
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
            StartCoroutine(Wait.WaitAndDo(startMove, () => newObject.AddComponent<M1001Object2>()));
            newObject.transform.position = pos;

        }

        End = false;
    }

    public override void StartPattern() {

        StartCoroutine(WaitAndSpawn(0.5f, 10));
    }
};