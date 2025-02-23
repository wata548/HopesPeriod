using System.Collections;
using DG.Tweening;
using UnityEngine;

public class M1001Pattern1: EnemyPatternBase {
    public override bool End { get; protected set; } = false;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    
    private static readonly Vector3 defaultPos = new(5.28f, 2.04f, -1);
    private static float boxSize = 2.5f;
    private static float startMove = 0.5f;
    private static int boxCount = 3;
    
    IEnumerator WaitAndSpawn(float waitTime, float time, int count) {

        int before = -1;
        int index = -1;
        End = true;
        for (int i = 0; i < count; i++) {

            yield return new WaitForSeconds(time * 1.1f + waitTime);


            while (before == index)
                index = Random.Range(0, boxCount);
            
            before = index;
            var pos = defaultPos;
            pos.y -= boxSize * index;

            Sequence animation = DOTween.Sequence();
            
            var rightObject = Instantiate(right);
            var leftObject = Instantiate(left);
            GameObject collider = null;
            rightObject.transform.localPosition = pos;
            pos.x *= -1;
            leftObject.transform.localPosition = pos;
            
            animation
                .Append(rightObject.transform.DOLocalMoveX(boxSize * 0.5f, time)
                    .SetEase(Ease.InQuint)
                    .DOBeforeWait(waitTime))
                .Join(leftObject.transform.DOLocalMoveX(-boxSize * 0.5f, time)
                    .SetEase(Ease.InQuint)
                    .DOBeforeWait(waitTime))
                
                .AppendCallback(() => {
                    collider = Instantiate(prefab);
                    collider.transform.localPosition = Vector3.up * pos.y;
                    collider.AddComponent<M1001Object1>();
                    ShakeCamera.Instance.Shake(0.6f, 0.1f);
                })
                .AppendInterval(waitTime)
                .AppendCallback(() => {
                    Destroy(collider);
                })
                
                .Append(rightObject.transform
                    .DOLocalMoveX(-pos.x, time * 0.65f)
                    .OnStart(() => Destroy(leftObject.GetComponent<Collider2D>())))
                .Join(rightObject.GetComponent<SpriteRenderer>().DOFade(0, time * 0.5f))
                .Join(leftObject.transform
                    .DOLocalMoveX(pos.x, time * 0.65f)
                    .OnStart(() => Destroy(rightObject.GetComponent<Collider2D>())))
                .Join(leftObject.GetComponent<SpriteRenderer>().DOFade(0, time * 0.5f))
                .OnComplete(() => {
                    Destroy(leftObject);
                    Destroy(rightObject);
                });
        }
        yield return new WaitForSeconds(time * 1.65f + waitTime + 2);

        End = false;
    }

    public override void StartPattern() {

        MapSizeManager.Instance.Default();
        StartCoroutine(WaitAndSpawn(0.3f, 0.6f, 15));
    }
}