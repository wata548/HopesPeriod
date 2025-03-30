using System;
using UnityEngine;

public class M1002Warnning: MonoBehaviour {

    public bool Follow { get; set; } = true;
    [field: SerializeField] public SpriteRenderer Renderer { get; private set; }
    private void Update() {
        if (!Follow)
            return;

        transform.localPosition = BaseEnemy.Player.transform.localPosition;
    }
}