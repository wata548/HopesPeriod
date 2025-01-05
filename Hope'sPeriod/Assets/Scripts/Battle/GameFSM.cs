using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameState {
    
    BattleStart,
    BeforeSkill,
    Skill,
    AfterSkill,
    PlayerAttack,
} 

    public class GameFSM: MonoBehaviour {

        public static GameFSM Instance { get; private set; } = null;

        private void Awake() {

            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(this);
        }

        public GameState State { get; private set; } = GameState.BattleStart;
        public bool isPattern = false;
        private void Update() {

            if (State == GameState.BattleStart) {
                State++;
            }

            if (State == GameState.BeforeSkill) {
                State++;
            }

            if (State == GameState.Skill) {

                if (!isPattern) {
                    
                    Monster.Instance.StartPattern();
                    isPattern = true;
                }
                if (!Monster.Instance.IsPattern) {
                    State++;
                    isPattern = false;
                }
            }

            if (State == GameState.AfterSkill) {
                State++;
            }

            if (State == GameState.PlayerAttack) {
                BaseEnemy.Player.GetComponent<PlayerPhysics>().Movement.SetApply<CompoInput>(Direction.None);
                //State = GameState.BeforeSkill;
            }
            
        }
    }