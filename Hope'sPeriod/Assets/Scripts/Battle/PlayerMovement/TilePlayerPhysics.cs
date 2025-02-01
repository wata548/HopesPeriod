using System;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Rigidbody2D))]
public class TilePlayerPhysics : MonoBehaviour {
    [SerializeField] private Image mapMoveEvent;
    [SerializeField] private GameObject map;
    [SerializeField] private int mapCode = 8401;
    private MapEventInfo mapInfo;
    private Vector2Int pos;

    Rigidbody2D playerRigidbody = null;
    private PlayerAnimation animation = null;

    private Direction moveableDirection = DirectionInfo.All;
    private Direction frictionDirection = DirectionInfo.All;

    private float power = 5f;
    private float frictionRatio = 0.5f;
    private float frictionPower = 20;

    private static readonly Vector2 ColliderSize = new (1, 0.5f);
    private static readonly Vector2 ColliderPos = new (0, -0.7f);
    private static readonly Vector2 SpriteSize = new(0.9f, 0.9f);
    private static readonly Vector2 DefaultPos = new(0, 0.8f);
    private static readonly Vector2 PivotPos = ColliderPos * SpriteSize + Vector2.one * 0.5f;
    
    Vector2 playerVelocity = Vector2.zero;

    private CompositeGroupBase movement;
    private void Awake() {

        animation ??= GetComponent<PlayerAnimation>();
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

        mapInfo = Resources.Load<MapEventInfo>("MapPrefab/MapInfo");
    }

    void Update() {

        var velocity = movement.Play(playerRigidbody.linearVelocity, Vector2.zero);
        playerRigidbody.linearVelocity = velocity;
        animation.SetAnimation(velocity);

        if (velocity != Vector2.zero) {
            var newPos = (transform.localPosition.ToVec2() + PivotPos).ToVec2Int();

            if (newPos != pos) {
                pos = newPos;
                if (mapInfo.ConnectInfo(mapCode, pos, out ConnectMapInfo connectMapInfo, out GameObject mapPrefab)) {

                    Destroy(map);
                    Debug.Log(mapPrefab);
                    map = Instantiate(mapPrefab);
                    mapCode = connectMapInfo.ConnectMapCode;
                    transform.localPosition = DefaultPos + connectMapInfo.ConnectPos;
                    pos = Vector2Int.zero;
                    
                    mapMoveEvent.color = Color.black;
                    mapMoveEvent.DOFade(0, 0.7f).SetEase(Ease.InCubic);
                }
            }
        }
    }
}