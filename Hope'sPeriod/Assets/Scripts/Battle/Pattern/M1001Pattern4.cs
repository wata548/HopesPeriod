using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

//Controle Player Gravity and Make Horizontal Attack
//Time: 2 * (0.5 * 10 + 2.5) = 15
public class M1001Pattern4: EnemyPatternBase {

    public override bool End { get; protected set; } = false;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject leftWarn;
    [SerializeField] private GameObject rightWarn;

    private void StartWarning(bool direction) {
        if (direction) {


            var right = rightWarn.GetComponentsInChildren<SpriteRenderer>();

            foreach (var factor in right) {
                factor.DOBlink(0.2f, 0.2f, 0.2f, 0.6f)
                    .OnComplete(() => factor.DOBlink(0.2f, 0.2f, 0.2f, 0.6f));
            }
        }
        else { 
            
            var left = leftWarn.GetComponentsInChildren<SpriteRenderer>();
            foreach (var factor in left) {
                factor.DOBlink(0.2f, 0.2f, 0.2f, 0.6f)
                    .OnComplete(() => factor.DOBlink(0.2f, 0.2f, 0.2f, 0.6f));
            }
        }
    }
    
    IEnumerator WaitAndSpawn(float time, int count) {

        End = true;

        
        bool direction = (Random.Range(0, 1 + 1) == 1);

        StartWarning(direction);
        yield return new WaitForSeconds(1f);
        Player.Instance.Movement
            .SetApply<CompoGravity>( direction ? Direction.Right : Direction.Left)
            .SetApply<CompoInput>(DirectionInfo.Vertical);
        
        for (int i = 0; i < count / 2; i++) {

            yield return new WaitForSeconds(time);
            var newObject = Instantiate(prefab);
            newObject.AddComponent<M1001Object4>();
            var a = newObject.GetComponent<M1001Object4>();
            a.SetPos(direction);

        }

        direction = !direction;
        yield return new WaitForSeconds(time * 4);
        StartWarning(direction);
        yield return new WaitForSeconds(0.7f);
        
        Player.Instance.Movement
            .SetApply<CompoGravity>(direction ? Direction.Right : Direction.Left);
        
        for (int i = 0; i < count / 2; i++) {

            yield return new WaitForSeconds(time);
            var newObject = Instantiate(prefab);
            newObject.AddComponent<M1001Object4>();
            var a = newObject.GetComponent<M1001Object4>();
            a.SetPos(direction);

        }

        Player.Instance.Movement
            .SetApply<CompoGravity>(Direction.None)
            .SetApply<CompoInput>(DirectionInfo.All);
        yield return new WaitForSeconds(time * 5);

        End = false;
    }

    public override void StartPattern() {
        
        MapSizeManager.Instance.Default();
        StartCoroutine(WaitAndSpawn(0.5f, 20));
    }
};