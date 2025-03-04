using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TestMove: MonoBehaviour {

    [SerializeField] private float power;
    private Rigidbody2D rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        var velo = Vector3.zero;
        velo.x += Input.GetAxisRaw("Horizontal");
        velo.y += Input.GetAxisRaw("Vertical");
        rigid.linearVelocity = velo * power;
    }
}