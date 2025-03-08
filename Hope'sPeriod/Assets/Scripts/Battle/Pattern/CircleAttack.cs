using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public abstract class CircleAttack: EnemyPatternBase {
    public IEnumerator CirclePattern<T>(
        int count, 
        float interval, 
        float stopTime,
        GameObject prefab,
        Vector3 centerPos,
        float radius = 1,
        List<Sprite> Sprites = null,
        bool useClockDirection = true,
        bool moveSameTime = false,
        float limitDegree = 2 * Mathf.PI, 
        float middleDegree = 0) where T: BaseEnemy {

        float intervalDegree = limitDegree / count;
        var startDegree = middleDegree + limitDegree * 0.5f;
        var clockDirection = false;
        if (useClockDirection) {
            
            clockDirection = Random.Range(0, 1f) > 0.5f ? true : false;
            if (clockDirection) {
                startDegree = middleDegree - limitDegree * 0.5f;
            }
        }

        Queue<GameObject> newObjects = new();
        for (int i = 0; i < count; i++) {
            float progress = i * intervalDegree;
            if (!clockDirection)
                progress *= -1;
            
            float current = startDegree + progress;
            var pos = centerPos + radius * new Vector3(Mathf.Cos(current), Mathf.Sin(current), -1);
            var newObject = GameObject.Instantiate(prefab);
            
            //random sprite
            if (Sprites is not null) {
                int index = Random.Range(0, Sprites.Count);
                newObject.GetComponent<SpriteRenderer>()
                    .sprite = Sprites[index];
            }
            
            newObject.transform.localPosition = pos;
            if(!moveSameTime)
                StartCoroutine(Wait.WaitAndDo(stopTime, 
                    () => newObject.AddComponent<T>()));

            yield return new WaitForSeconds(interval);
        }

        if (moveSameTime) {
            yield return new WaitForSeconds(stopTime);
            while (newObjects.Count > 0) {
                newObjects.Peek().AddComponent<T>();
                newObjects.Dequeue();
            }
        }
        
    }        
}