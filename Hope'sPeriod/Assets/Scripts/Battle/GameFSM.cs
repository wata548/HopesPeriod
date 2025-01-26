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

public enum PlayerTurnState {
    
    SelectBehavior,
    Attack,
    Item,
    ItemTarget,
    Shield
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

    public GameState State { get; private set; } = GameState.PlayerAttack;
    public PlayerTurnState PlayerTurnState { get; private set; } = PlayerTurnState.SelectBehavior;
    
    //Check during pattern
    private bool isPattern = false;
    //when player turn if it's value is false, map and player position will be refreshed; 
    private bool playerTurnStart = false;
    private bool needPlayerTurnUpdate = true;
    
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

            PlayerTurnStartSetting();

            if (needPlayerTurnUpdate && PlayerTurnState != PlayerTurnState.SelectBehavior) {

                needPlayerTurnUpdate = false;
                
                switch (PlayerTurnState) {
                    
                    case PlayerTurnState.Attack:
                        SkillButtonManager.Instance.TurnOn();
                        break;
                    case PlayerTurnState.Item:
                        ItemListButtonManager.Instance.TurnOn();
                        break;
                    case PlayerTurnState.ItemTarget:
                        
                        int index = SelectCursor.Instance.Index;
                        var target = ControleCharacterInfo.Instance.CharacterInfo(index);
                        int code = TargetButtonManager.Instance.Code;

                        Inventory.UseItem(code, target);
                        break;
                    case PlayerTurnState.Shield:
                        ControleCharacterInfo.Instance.ShieldOn();
                        SkipState();
                        break;
                    default:
                        throw new OutOfRange(
                            (int)PlayerTurnState.Attack, 
                            (int)PlayerTurnState.Shield,
                            (int)PlayerTurnState, 
                            "PlayerTurnState must be Attack or Item or Shield"
                        );
                } 
            }

            PlayerTurnInput();
        }
            
    }

    private void PlayerTurnStartSetting() {
        if (!playerTurnStart) {
            playerTurnStart = true;
            PlayerTurnState = PlayerTurnState.SelectBehavior;
                        
            Player.Instance.Object.transform.DOLocalMove(selectPlayerPos, 0.5f);
            MapSizeManager.Instance.Move(selectMapPos);
            MapSizeManager.Instance.Resize(selectMapScale);
                            
            Player.Instance.Movement
                .SetApply<CompoInput>(Direction.None);
        }
    }
    
    private void PlayerTurnInput() {
        bool isChossingTarget = TargetButtonManager.Instance.Interactable;
        bool isSelectState = PlayerTurnState == PlayerTurnState.SelectBehavior;
        if (isChossingTarget || isSelectState) return;
        
    }

    public void DefaultPlayerTurnState() {
        
        ItemListButtonManager.Instance.TurnOff();
        SkillButtonManager.Instance.TurnOff();
                    
        needPlayerTurnUpdate = true;
        PlayerTurnState = PlayerTurnState.SelectBehavior;
    }
    
    public void SetPlayerTurnState(PlayerTurnState newState) {

        bool isPlayerTurn = State == GameState.PlayerAttack;
        bool isSelectState = PlayerTurnState == PlayerTurnState.SelectBehavior;
        if (!isPlayerTurn || !isSelectState)
            return;

        PlayerTurnState = newState;
        Debug.Log($"click {newState.ToString()}");
    }

    public void AfterSetTarget() {

        if (PlayerTurnState == PlayerTurnState.Item) {
            PlayerTurnState = PlayerTurnState.ItemTarget;
            needPlayerTurnUpdate = true;
        }
    }
    public void ClearPlayerTurnState() {
        bool isPlayerTurn = State == GameState.PlayerAttack;
        if (!isPlayerTurn)
            return;

        PlayerTurnState = PlayerTurnState.SelectBehavior;
    }

    public void SkipState() {
        if (State == GameState.PlayerAttack) {

            EndPlayerTurn();
            needPlayerTurnUpdate = true;
        }
        else {
            State++;
        }
    }

    private void EndPlayerTurn() {
        
        playerTurnStart = false;
        Player.Instance.Movement
            .SetApply<CompoInput>(DirectionInfo.All);
        
        ItemListButtonManager.Instance.TurnOff();
        SkillButtonManager.Instance.TurnOff();
        ControleCharacterInfo.Instance.TurnUpdate();
         
        State = GameState.BeforeSkill;
        PlayerTurnState = PlayerTurnState.SelectBehavior;
    }
}