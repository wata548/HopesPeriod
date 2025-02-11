using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameState {
    
    BattleStart,
    BeforeSkill,
    MonsterSkill,
    AfterSkill,
    PlayerAttack,
    Win
}

public enum PlayerTurnState {
    
    SelectBehavior,
    Attack,
    AttackTarget,
    Item,
    ItemTarget,
    Shield
}

public class GameFSM: MonoBehaviour {

    [SerializeField] private SkillShower skillShower;
    private readonly Vector3 selectPlayerPos = new(0, 0, -1);
    private readonly Vector3 selectMapPos = new(0, 0.35f, -0.7f);
    private readonly Vector2 selectMapScale = new(13, 6);
        
    public static GameFSM Instance { get; private set; } = null;

    private void Awake() {

        Instance = this;

        State = GameState.BattleStart;
        PlayerTurnState = PlayerTurnState.SelectBehavior;
    }

    public GameState State { get; private set; } = GameState.PlayerAttack;
    public PlayerTurnState PlayerTurnState { get; private set; } = PlayerTurnState.SelectBehavior;
    
    //Check during pattern
    private bool isPattern = false;
    //when player turn if it's value is false, map and player position will be refreshed; 
    private bool playerTurnStart = false;
    private bool needPlayerTurnUpdate = true;

    private bool end = false;
    private void Update() {

        if (MonsterSlider.Instance.Dead) {
            
            State = GameState.Win;
        }
            
        if (State == GameState.BattleStart) {
            SkipState();
        }

        else if (State == GameState.BeforeSkill) {
            SkipState();
        }

        else if (State == GameState.MonsterSkill) {

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

            PlayerTurnFSM();
        } 
        else if (!end && State == GameState.Win) {

            ScenceControler.Load("PlayMap");
            SkillButtonManager.Instance.TurnOff();
            end = true;
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

    public void PlayerTurnFSM() {

        if (!needPlayerTurnUpdate || PlayerTurnState == PlayerTurnState.SelectBehavior) return;
        needPlayerTurnUpdate = false;
        
        switch (PlayerTurnState) {
                            
            case PlayerTurnState.Attack:
                SkillButtonManager.Instance.TurnOn();
                break;
            case PlayerTurnState.AttackTarget:
                int selectIndex = SelectCursor.Instance.Index;
                SkillButtonManager.Instance.SetInteractable(true);
                SkillButtonManager.Instance.NextSelect(selectIndex);
                PlayerTurnState = PlayerTurnState.Attack;
                break;
            
            case PlayerTurnState.Item:
                ItemListButtonManager.Instance.TurnOn();
                break;
            case PlayerTurnState.ItemTarget:
                int index = SelectCursor.Instance.Index;
                var target = CharactersInfoBattle.Instance.CharacterInfo(index);
                int code = TargetButtonManager.Instance.Code;
        
                Inventory.UseItemBattle(code, target);
                break;
            
            case PlayerTurnState.Shield:
                CharactersInfoBattle.Instance.ShieldOn();
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
    
    public void DefaultPlayerTurnState() {
        
        ItemListButtonManager.Instance.TurnOff();
        SkillButtonManager.Instance.TurnOff();
                    
        needPlayerTurnUpdate = true;
        PlayerTurnState = PlayerTurnState.SelectBehavior;
    }

    public void Win() {
        State = GameState.Win;
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

        if (PlayerTurnState == PlayerTurnState.Attack) {
            PlayerTurnState = PlayerTurnState.AttackTarget;
            needPlayerTurnUpdate = true;
        }
    }

    public void SkipState() {

        if (State == GameState.PlayerAttack) {
            SkipStateProcess();
            skillShower.Show().OnComplete(() => {
                    
                    //Moveable
                    Player.Instance.Movement
                        .SetApply<CompoInput>(DirectionInfo.All);
                    
                    //State set
                    State = GameState.BeforeSkill;
                    PlayerTurnState = PlayerTurnState.SelectBehavior;
                    
                    //express need to update
                    playerTurnStart = false;
                    needPlayerTurnUpdate = true;

                    skillShower.UpdateRemainInfo();
                }
            );
        }
        else {
            SkipStateProcess();
        }
    } 
    
    private void SkipStateProcess() {
        if (State == GameState.PlayerAttack) {

            Disinteractable();
        }
        else {
            State++;
        }
    }

    private void Disinteractable() {
        
        ItemListButtonManager.Instance.TurnOff();
        SkillButtonManager.Instance.TurnOff();
        CharactersInfoBattle.Instance.TurnUpdate();
         
    }
}