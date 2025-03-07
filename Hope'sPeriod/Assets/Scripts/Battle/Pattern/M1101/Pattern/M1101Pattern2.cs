using System.Collections;
using UnityEngine;

public class M1101Pattern2: EnemyPatternBase {
    public override bool Active { get; protected set; }
    [SerializeField] private GameObject prefab;

    private IEnumerator Generate(int repeat, float radius, int count, float interval, float showTime) {
        Active = true;
        float degreeInterval = (2 * Mathf.PI) / count;

        while (repeat-- != 0) {

            bool clock = Random.Range(0, 1f) < 0.5f ? true : false;
            for (int i = 0; i < count; i++) {
                float degree = i * degreeInterval;

                if (!clock)
                    degree = 2 * Mathf.PI - degree;
                var pos = radius * new Vector3(Mathf.Cos(degree), Mathf.Sin(degree), -1);
                var bullet = Instantiate(prefab);
                bullet.transform.localPosition = pos;
                StartCoroutine(Wait.WaitAndDo(showTime, () => bullet.AddComponent<M1101Object2>()));
                yield return new WaitForSeconds(interval);
            }

            yield return new WaitForSeconds(interval * count + showTime * 0.5f);

        }

        yield return new WaitForSeconds(0.5f * showTime);
        Active = false;
    }
    
    public override void StartPattern() {
        MapSizeManager.Instance.Resize(new(5, 5));
        //MapSizeManager.Instance.Move(ne)
        StartCoroutine(Generate(3, 5, 8, 0.13f, 0.9f));
    }

    public override void GrouptPattern() { }
}