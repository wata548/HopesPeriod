using System;
using UnityEngine;
public class M11013Warnning : MonoBehaviour {
    private void Update() {
        transform.localPosition = BaseEnemy.Player.transform.localPosition;
    }
}