using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class SettingCollider
{
    private const float RealSize            = 0.8f;
    private const float CheckColliderRange  = 0.05f;
    private const float SpareLength         = 0.05f;
    public ContactInfo ContactInfo { get; private set; } = new();

    public SettingCollider(GameObject player) {
        SetCollider(player, Vector2.one, Vector2. zero);
    }

    public SettingCollider(GameObject player, Vector2 defaultSize, Vector2 offset) {
        SetCollider(player, defaultSize, offset);
    }

    public void SetCollider(GameObject player, Vector2 defaultSize, Vector2 offset) {

        //* Generate player's collider
        var playerCollider = player.AddComponent<BoxCollider2D>();
        playerCollider.size = RealSize * defaultSize;
        playerCollider.offset = offset;

        //* Generate contact checker
        Vector2 pos;
        Vector2 size;

        Vector2 distance = (RealSize / 2 * defaultSize);
        distance.x += CheckColliderRange / 2;
        distance.y += CheckColliderRange / 2;

        pos = new Vector2(distance.x, 0);
        size = new Vector2(CheckColliderRange, RealSize - SpareLength) * defaultSize;
        GenerateContactChecker(player, pos + offset, size, Direction.Right);
        GenerateContactChecker(player, -pos + offset, size, Direction.Left);

        pos = new Vector2(0, distance.y);
        size = new Vector2(RealSize - SpareLength, CheckColliderRange) * defaultSize;
        GenerateContactChecker(player, pos + offset, size, Direction.Up);
        GenerateContactChecker(player, -pos + offset, size, Direction.Down);

    }

    //* It make gameObject that will check state(associate the wall)
   private GameObject GenerateContactChecker(GameObject player, Vector2 pos, Vector2 size, Direction dir) {

        GameObject checker = new();
        checker.name = $"{Enum.GetName(typeof(Direction), dir)}contactChecker";

        Transform checkerTransform = checker.transform;
        checkerTransform.parent = player.transform;
        checkerTransform.localPosition = Vector2.zero;
        checkerTransform.localScale = Vector2.one;

        var checkerCollider = checker.AddComponent<BoxCollider2D>();
        checkerCollider.isTrigger = true;

        checkerCollider.size = size;
        checkerCollider.offset = pos;
        
        checker.AddComponent<ContactChecker>()
            .SetDirection(dir)
            .SetContactInfo(this.ContactInfo);

        return checker;
    }
}