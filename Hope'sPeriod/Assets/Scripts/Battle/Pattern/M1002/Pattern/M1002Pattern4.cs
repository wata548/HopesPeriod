using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//trace trap
public class M1102Pattern4: EnemyPatternBase {
    public override bool Active { get; protected set; }
    [SerializeField] private GameObject patternObject;
    [SerializeField] private M1002Warnning warnning;
    
    private IEnumerator Pattern(int count) {
        Active = true;
        warnning.gameObject.SetActive(true);

        List<GameObject> generated = new();
        while (count-- != 0) {

            warnning.Renderer
                .DOBlink(0.3f, 0.1f, 0.3f, 0.8f)
                .DORepeat(2);
            yield return new WaitForSeconds(1.4f);
            warnning.Follow = false;
            warnning.Renderer
                .DOBlink(0.1f, 0.1f, 0.1f, 0.8f)
                .DORepeat(3);
            var pos = BaseEnemy.Player.transform.localPosition;
            yield return new WaitForSeconds(0.85f);
            warnning.Follow = true;
            
            var newObject = Instantiate(patternObject);
            pos.z += 0.01f;
            newObject.transform.localPosition = pos;
            generated.Add(newObject);
            
            yield return new WaitForSeconds(0.1f);
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
        StartCoroutine(Pattern(5));
    }

    public override void GrouptPattern() {
    }
}
