using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AutoAnimator: MonoBehaviour {
     
     private Animator animator;

     private void Awake() {
          
          animator = GetComponent<Animator>();
          var route = ((MoveScriptCommand)ScriptCode.Interpret(@"Move(Target = 123| Route = [L,L,L + U,L + D,L+D,L+U]);")[0].Item2).Route;
          SetRoute(route, 0.5f);
     }
     
     private void SetRoute(List<Direction> route, float duraction) {

          Sequence animation = DOTween.Sequence();
          Vector3 pos = gameObject.transform.localPosition;
          
          foreach (var dir in route) {
               Vector3 direction = dir.ConvertVector().ToVec3(); 
               pos += direction;
               animation.Append(gameObject.transform
                    .DOLocalMove(pos, duraction * dir.SimpleDirection())
                    .SetEase(Ease.Linear)
                    .OnStart(() => {
                         animator.SetFloat("Horizontal", direction.x);
                         animator.SetFloat("Vertical", direction.y);
                         animator.SetFloat("Speed", direction.magnitude);
                         Debug.Log($"{animator.GetFloat("Horizontal")} + {animator.GetFloat("Vertical")} = {animator.GetFloat("Speed")}");
                    }) 
               );
          }
          
          //TODO: End Process
          animation.OnComplete(() => Debug.Log("end"));
     }
}