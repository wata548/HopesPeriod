using System;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Rigidbody2D))]
public class TilePlayerPhysics : MonoBehaviour {
    
    [SerializeField] private Image mapMoveEffect;
    private Vector2Int pos;
    public static bool Movable { get; private set; } = true;
    private static PlayerAnimation animation = null;

    public static void SetMovable(bool target) {

        animation.SetOn(target);
        Movable = target;
    }
    
    Rigidbody2D playerRigidbody = null;

    private Direction moveableDirection = DirectionInfo.All;
    private Direction frictionDirection = DirectionInfo.All;

    private float power = 5f;
    private float frictionRatio = 0.5f;
    private float frictionPower = 20;

    private static readonly Vector2 ColliderSize = new (1, 0.5f);
    private static readonly Vector2 ColliderPos = new (0, 0.13f);
    private static readonly Vector2 SpriteSize = new(0.9f, 0.9f);
    private static readonly Vector2 PivotPos = ColliderPos * SpriteSize + Vector2.one * 0.5f;
    
    Vector2 playerVelocity = Vector2.zero;

    private CompositeGroupBase movement;
    private void Awake() {

        int code;
        (code, pos) = SaveMapInfo.Load();
        
        animation = GetComponent<PlayerAnimation>();
        movement = new CompositeGroupBase(gameObject)
            .SetCollider(new SettingCollider(gameObject, ColliderSize, ColliderPos))
            .AddComposite(new CompoInput(null))
            .AddComposite(new CompoFriction(null))
            .SetApply<CompoInput>(moveableDirection)
            .SetPower<CompoInput>(power)
            .SetApply<CompoFriction>(frictionDirection)
            .SetPower<CompoFriction>(frictionPower * frictionRatio);
        
        if (playerRigidbody == null) {

            playerRigidbody = GetComponent<Rigidbody2D>();
        }

        ItemInfo.SetTable();
        SkillInfo.SetTable();
        
        CheckEvent.SetEffect(mapMoveEffect);
        CheckEvent.SetMap(code);
        transform.localPosition = pos.ToVec2();
    }

    void Update() {

        if (!Movable) {
            playerRigidbody.linearVelocity = Vector2.zero;
            animation.SetSpeed(Vector2.zero);
            return;
        }
        
        var velocity = movement.Play(playerRigidbody.linearVelocity, Vector2.zero);
        playerRigidbody.linearVelocity = velocity;
        animation.SetSpeed(velocity);

        if (velocity != Vector2.zero) {
            var newPos = (transform.localPosition.ToVec2() + PivotPos).ToVec2Int();

            if (newPos != pos) {
                pos = newPos;
                CheckEvent.CheckAutoEvent(ref pos, gameObject);
                MeetMonsterEvent(CheckEvent.MeetMonster());
            }
        }

        CheckEvent.CheckInteract(pos, animation.Dir);

    }
    
    
    private void MeetMonsterEvent(int code) {
        if (code == 0)
            return;

        SaveMapInfo.Save(CheckEvent.MapCode, pos);
        SetMovable(false);
        Debug.Log($"Meet Monster {code}");
        ShakeCamera.Instance.HShake(0.5f, 0.2f);
        ScenceChangeEffecter.Instance.StartEffect()
            .OnComplete(() => ScenceControler.Load("Battle"));
    }
}