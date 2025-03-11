using System.Collections;
using UnityEngine;

//trace trap
public class M1101Pattern3: EnemyPatternBase {
    public override bool Active { get; protected set; }
    [SerializeField] private SpriteRenderer warnning;
    [SerializeField] private GameObject patternObject;
    
    private IEnumerator Pattern(int count, float waitTime) {
        Active = true;
        warnning.gameObject.SetActive(true);
        while (count-- != 0) {

            warnning
                .DOBlink(0.3f, 0.1f, 0.3f)
                .DORepeat(2);
            yield return new WaitForSeconds(1.4f);
            var pos = BaseEnemy.Player.transform.localPosition;
            yield return new WaitForSeconds(0.1f);

            var newObject = Instantiate(patternObject);
            pos.z = -1;

            newObject.transform.localPosition = pos;
            StartCoroutine(Wait.WaitAndDo(waitTime, () => Destroy(newObject)));
        }

        warnning.gameObject.SetActive(false);
        Active = false;
    }
    
    public override void StartPattern() {
        MapSizeManager.Instance.Default();
        StartCoroutine(Pattern(10, 10));
    }

    public override void GrouptPattern() {
        StartCoroutine(Pattern(10, 10));
    }
}