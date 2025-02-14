using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AutoAnimator: MonoBehaviour {
     
     private Animator animator;

     private void Awake() {
          
          animator = GetComponent<Animator>();
     }

     private static readonly Dictionary<Direction, Vector3> directionVector = new() {
          { Direction.Down, Vector3.down},
          { Direction.Left, Vector3.left},
          { Direction.Right, Vector3.right},
          { Direction.Up, Vector3.up}
     };
     
     private void SetRoute(List<Direction> route, float duraction, ref bool end) {

          Sequence animation = DOTween.Sequence();
          foreach (var dir in route) {
               animation.Append(gameObject.transform.DOMove(directionVector[dir], duraction));
          }
     }
}