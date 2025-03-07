using UnityEngine;

public class TestPattern: CircleAttack {
    public override bool Active { get; protected set; }
    [SerializeField] private GameObject prefabs;
    public override void StartPattern() {
        StartCoroutine(CirclePattern<M1101Object2>(14, 0.1f, 0.5f, prefabs, Vector3.left * 5, 3));
    }

    public override void GrouptPattern() {
        throw new System.NotImplementedException();
    }
}