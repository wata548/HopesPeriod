using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


//Throwing knife
public class M1002Pattern2: EnemyPatternBase {
    public override bool Active { get; protected set; } = false;
    [SerializeField] private List<Sprite> images;
    [SerializeField] private GameObject prefab;
    
    //0.6 * 10 + 2 = 8
    private IEnumerator Pattern(int repeat,float firstRadius, float radius, float appear, float moveTiming) {

        Active = true;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < repeat; i++) {
            yield return new WaitForSeconds(moveTiming / 4);
            
            //datas
            var playerPos = BaseEnemy.Player.transform.localPosition;
            var degree = Random.Range(-0.7f, 0.7f) + (Random.Range(0,2) == 1 ? Mathf.PI : 0);
            var direction = new Vector3(Mathf.Cos(degree), Mathf.Sin(degree));
            var generatedPos = firstRadius * direction + playerPos; 
            var startPos = radius * direction + playerPos;

            //generate
            var newObject = Instantiate(prefab);
            newObject.transform.rotation = Quaternion.Euler(0, 0, 180 + degree * Mathf.Rad2Deg);
            newObject.transform.localPosition = generatedPos;
            var sprite = images[Random.Range(0, images.Count)];
            newObject.GetComponent<SpriteRenderer>().sprite = sprite; 
            
            //object move 
            newObject.transform.DOMove(startPos, appear)
                .SetEase(Ease.OutQuint);
            var objectPatten = newObject.GetComponent<M1002Object2>();
            objectPatten.TargetPos = playerPos;
            
            yield return new WaitForSeconds(appear + moveTiming);
            //move start
            objectPatten.MoveStart = true;
        }

        yield return new WaitForSeconds(1.5f);
        Active = false;
    }
    
    public override void StartPattern() {
        MapSizeManager.Instance.Resize(new Vector3(11.25f, 6.5f));
        MapSizeManager.Instance.Move();
        StartCoroutine(Pattern(10, 7f, 4.5f, 0.4f, 0.2f));
    }

    public override void GrouptPattern() {
    }
}