using System;
using UnityEngine;

public class SettingCollider
{
    private const float RealSize            = 0.8f;
    private const float CheckColliderRange  = 0.05f;
    private const float SpareLength         = 0.05f;
    public ContactInfo ContactInfo { get; private set; } = new();
    
    public void SetCollider(GameObject player) {

        //* Generate player's collider
        var playerCollider = player.AddComponent<BoxCollider2D>();
        playerCollider.size = Vector2.one * RealSize;

        //* Generate contact checker
        Vector2 pos;
        Vector2 size;

        float distance = RealSize / 2 + CheckColliderRange / 2;

        pos = new Vector2(distance, 0);
        size = new Vector2(CheckColliderRange, RealSize - SpareLength);
        GenerateContactChecker(player, pos, size, Direction.Right);
        GenerateContactChecker(player, -pos, size, Direction.Left);

        pos = new Vector2(0, distance);
        size = new Vector2(RealSize - SpareLength, CheckColliderRange);
        GenerateContactChecker(player, pos, size, Direction.Up);
        GenerateContactChecker(player, -pos, size, Direction.Down);

    }

    //* It make gameObject that will check state(associate the wall)
    private GameObject GenerateContactChecker(GameObject player, Vector2 pos, Vector2 size, Direction dir) {

        GameObject checker = new();
        checker.name = $"{Enum.GetName(typeof(Direction), dir)}contactChecker";

        Transform checkerTransform = checker.transform;
        checkerTransform.parent = player.transform;
        checkerTransform.position = Vector2.zero;
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