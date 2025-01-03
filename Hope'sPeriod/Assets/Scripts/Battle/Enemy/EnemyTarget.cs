using UnityEngine;

public class EnemyTarget: MonoBehaviour {

    [SerializeField] private GameObject player;
    private void Awake() {
        BaseEnemy.SetPlayer(player);
    }
}