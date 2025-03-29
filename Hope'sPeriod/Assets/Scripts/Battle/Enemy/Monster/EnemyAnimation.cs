using System;
using DG.Tweening;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
public enum PlayerTurnEnemyAnimationState {
    On,
    Off,
    Default
}

public abstract class EnemyAnimation : MonoBehaviour {
    
    public abstract bool UseStartEvent { get; }
    private PlayerTurnEnemyAnimationState state = PlayerTurnEnemyAnimationState.Off; 
    private bool isActive = false;
    private Tween _currentAnimation = null;

    public abstract Tween OnAnimaton();
    public abstract Tween StayAnimaton();
    public abstract Tween OutAnimaton();

    protected virtual void On() {

        if (_currentAnimation is not null)
            _currentAnimation.Kill();
        Tween ani = OnAnimaton();
        _currentAnimation = ani;
        
        if (ani is null) {

            state = PlayerTurnEnemyAnimationState.Default;
            return;
        }

        ani.OnComplete(() => state = PlayerTurnEnemyAnimationState.Default);
    }
    protected virtual void Out() {
        if (_currentAnimation is not null)
            _currentAnimation.Kill();
        _currentAnimation = OutAnimaton();
    }
    protected virtual void Stay() {
        StayAnimaton();
    }
    
    private void StartAnimation() {

        switch (state) {
            case PlayerTurnEnemyAnimationState.Default:
                Stay();
                break;
            case PlayerTurnEnemyAnimationState.On:
                On();
                break;
            case PlayerTurnEnemyAnimationState.Off:
                Out();
                break;
        }
    }

    public void SetState() {

        bool isPlayerTurn = GameFSM.Instance.State == GameState.PlayerAttack;
        bool isGameStart = UseStartEvent && GameFSM.Instance.State == GameState.BattleStart;

        if (!isActive && (isPlayerTurn || isGameStart)) {
            Debug.Log("start");
            isActive = true;
            state = PlayerTurnEnemyAnimationState.On;
            StartAnimation();
        }

        if (isActive && !isPlayerTurn && !isGameStart) {
            isActive = false;
            state = PlayerTurnEnemyAnimationState.Off;
            StartAnimation();
        }

        if (state == PlayerTurnEnemyAnimationState.Default) {
            
            StartAnimation();
        }
    }

    private void Update() {
        SetState();
    }
}