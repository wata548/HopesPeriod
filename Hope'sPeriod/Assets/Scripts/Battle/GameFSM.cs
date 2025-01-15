using System;
using DG.Tweening;
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

    private readonly Vector3 selectPlayerPos = new(0, 0, -1);
    private readonly Vector3 selectMapPos = new(0, 0.35f, -0.7f);
    private readonly Vector2 selectMapScale = new(13, 6);
        
    public static GameFSM Instance { get; private set; } = null;

    private void Awake() {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public GameState State { get; private set; } = GameState.BattleStart;
    public PlayerTurnState PlayerTurnState { get; private set; } = PlayerTurnState.SelectBehavior;
    
    private bool isPattern = false;
    private bool isPlayerTurnStart = false;
    private void Update() {

        if (State == GameState.BattleStart) {
            SkipState();
        }

        else if (State == GameState.BeforeSkill) {
            SkipState();
        }

        else if (State == GameState.Skill) {

            if (!isPattern) {
                    
                Monster.Instance.StartPattern();
                isPattern = true;
            }
            if (!Monster.Instance.IsPattern) {
                State++;
                isPattern = false;
            }
        }

        else if (State == GameState.AfterSkill) {
            SkipState();
        }

        else if (State == GameState.PlayerAttack) {

            if (!isPlayerTurnStart) {
                isPlayerTurnStart = true;

                Player.Instance.Object.transform.DOLocalMove(selectPlayerPos, 0.5f);
                MapSizeManager.Instance.Move(selectMapPos);
                MapSizeManager.Instance.Resize(selectMapScale);
                    
                Player.Instance.Movement
                    .SetApply<CompoInput>(Direction.None);
            }
        }
            
    }

    public void SelectPlayerTurnState(PlayerTurnState newState) {

        bool gameState = State != GameState.PlayerAttack;
        bool playState = PlayerTurnState != PlayerTurnState.SelectBehavior;
        if (gameState || playState)
            return;

        PlayerTurnState = newState;
        Debug.Log($"click {newState.ToString()}");
        //TODO: interact
    }

    public void ClearPlayerTurnState() {
        bool gameState = State != GameState.PlayerAttack;
        if (gameState)
            return;

        PlayerTurnState = PlayerTurnState.SelectBehavior;
    }

    public void SkipState() {
        if (State == GameState.PlayerAttack) {

            isPlayerTurnStart = false;
            Player.Instance.Movement
                .SetApply<CompoInput>(DirectionInfo.All);
            State = GameState.BeforeSkill;
        }
        else {
            State++;
        }
    }
}