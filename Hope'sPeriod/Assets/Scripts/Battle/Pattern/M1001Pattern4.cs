using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

//Controle Player Gravity and Make Horizontal Attack
//Time: 15
public class M1001Pattern4: EnemyPatternBase {

    public override bool End { get; protected set; } = false;
    [SerializeField] private GameObject prefab;
    IEnumerator WaitAndSpawn(float time, int count) {

        End = true;
        
        bool direction = (Random.Range(0, 1 + 1) == 1);
        BaseEnemy.Player.GetComponent<PlayerPhysics>().Movement
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
        BaseEnemy.Player.GetComponent<PlayerPhysics>().Movement
            .SetApply<CompoGravity>(direction ? Direction.Right : Direction.Left);

        yield return new WaitForSeconds(time * 5);
        for (int i = 0; i < count / 2; i++) {

            yield return new WaitForSeconds(time);
            var newObject = Instantiate(prefab);
            newObject.AddComponent<M1001Object4>();
            var a = newObject.GetComponent<M1001Object4>();
            a.SetPos(direction);

        }

        BaseEnemy.Player.GetComponent<PlayerPhysics>().Movement
            .SetApply<CompoGravity>(Direction.None)
            .SetApply<CompoInput>(DirectionInfo.All);
        yield return new WaitForSeconds(time * 5);

        End = false;
    }

    public override void StartPattern() {
        
        StartCoroutine(WaitAndSpawn(0.5f, 20));
    }
};