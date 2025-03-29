using System.Collections;
using DG.Tweening;
using UnityEngine;

//around circle trace
public class M1101Pattern2: EnemyPatternBase {
    public override bool Active { get; protected set; }
    [SerializeField] private GameObject prefab;

    private IEnumerator Generate(int repeat, float startRadius, float radius, int count, float interval, float showTime) {
        Active = true;
        float degreeInterval = (2 * Mathf.PI) / count;

        while (repeat-- != 0) {

            bool clock = Random.Range(0, 1f) < 0.5f ? true : false;
            for (int i = 0; i < count; i++) {
                float degree = i * degreeInterval;

                if (!clock)
                    degree = 2 * Mathf.PI - degree;
                var startPos = startRadius * new Vector3(Mathf.Cos(degree), Mathf.Sin(degree), -1);
                var pos = radius * new Vector3(Mathf.Cos(degree), Mathf.Sin(degree), -1);
                var bullet = Instantiate(prefab);

                bullet.transform.localRotation = Quaternion.Euler(0, 0, 180 + Mathf.Atan2(startPos.y, startPos.x) * Mathf.Rad2Deg);
                bullet.transform.localPosition = startPos;
                bullet.transform.DOMove(pos, showTime / 2);
                
                StartCoroutine(Wait.WaitAndDo(showTime, () => bullet.AddComponent<M1101Object2>()));
                yield return new WaitForSeconds(interval);
            }

            yield return new WaitForSeconds(interval * count + showTime * 0.5f);

        }

        yield return new WaitForSeconds(0.7f * showTime);
        Active = false;
    }
    
    public override void StartPattern() {
        MapSizeManager.Instance.Resize(new Vector2(5f, 5f));
        //MapSizeManager.Instance.Move(ne)
        StartCoroutine(Generate(3, 8, 5, 8, 0.13f, 0.9f));
    }

    public override void GrouptPattern() { }
}