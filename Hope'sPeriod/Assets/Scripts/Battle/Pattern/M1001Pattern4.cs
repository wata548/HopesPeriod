using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class M1001Pattern4: MonoBehaviour {

    public bool End { get; private set; } = false;
    [SerializeField] private GameObject prefab;
    [SerializeField] private SpriteRenderer[] warning = new SpriteRenderer[3];
    private const float startMove = 1.5f;
    private const float widthCount = 4;
    private const float heightCount = 6;
    
    IEnumerator WaitAndSpawn(float time, int count) {

        End = true;
        
        bool direction = (Random.Range(0, 1 + 1) == 1);
        BaseEnemy.Player.GetComponent<PlayerPhysics>().Movement
            .SetApply<CompoGravity>( direction ? Direction.Right : Direction.Left)
            .SetApply<CompoInput>(DirectionInfo.Vertical);
        
        for (int i = 0; i < count; i++) {

            var pos = new Vector3(-2.5f, 3.5f, -1);
            int part = Random.Range(-1, 1 + 1);

            pos.x += part * widthCount;

            yield return new WaitForSeconds(0.5f);
            var newObject = Instantiate(prefab);
            newObject.AddComponent<M1001Object4>().SetPos(direction);
        }

        End = false;
    }

    public void StartPattern() {
        
        StartCoroutine(WaitAndSpawn(0.5f, 100));
    }

    //test
    private void Start() {
        StartPattern();
    }

};