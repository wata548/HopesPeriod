using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//trace trap
public class M1101Pattern3: EnemyPatternBase {
    public override bool Active { get; protected set; }
    [SerializeField] private SpriteRenderer warnning;
    [SerializeField] private GameObject patternObject;
    
    //1.75 * 9 + 2 = 17.75
    private IEnumerator Pattern(int count, float waitTime) {
        Active = true;
        warnning.gameObject.SetActive(true);

        List<GameObject> generated = new();
        while (count-- != 0) {

            warnning
                .DOBlink(0.3f, 0.1f, 0.3f, 0.8f)
                .DORepeat(2);
            yield return new WaitForSeconds(1.4f);
            var pos = BaseEnemy.Player.transform.localPosition;
            yield return new WaitForSeconds(0.35f);

            var newObject = Instantiate(patternObject);
            pos.z = -1;
            generated.Add(newObject);
            
            newObject.transform.localPosition = pos;
            StartCoroutine(Wait.WaitAndDo(waitTime - 1, () => {

                if (newObject == null)
                    return;
                
                newObject?.GetComponent<Animator>()?.SetBool("Dis", true);
                StartCoroutine(Wait.WaitAndDo(1, () => {
                    if (newObject != null)
                        Destroy(newObject);
                }));
            }));
        }

        warnning.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        foreach (var newObject in generated) {
            Destroy(newObject);
        }
        
        Active = false;
    }
    
    public override void StartPattern() {
        MapSizeManager.Instance.Default();
        StartCoroutine(Pattern(10, 10));
    }

    public override void GrouptPattern() {
        StartCoroutine(Pattern(9, 9));
    }
}