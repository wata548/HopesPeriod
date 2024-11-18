using System;
using UnityEngine;

public class SettingCollider
{
    const float PLAYER_SIZE          = 0.7f;
    const float CHECK_COLLIDER_RANGE = 0.05f;
    const float SPARE_LENGTH         = 0.05f;
    
    public static void SetCollider(GameObject player) {


        //* Generate player's collider
        var playerCollider = player.AddComponent<BoxCollider2D>();
        playerCollider.size = Vector2.one * PLAYER_SIZE;

        //* Generate contact checker
        Vector2 pos;
        Vector2 size;

        float distance = PLAYER_SIZE / 2 + CHECK_COLLIDER_RANGE / 2;

        pos = new Vector2(distance, 0);
        size = new Vector2(CHECK_COLLIDER_RANGE, PLAYER_SIZE - SPARE_LENGTH);
        GenerateContactChecker(player, pos, size, Direction.RIGHT);
        GenerateContactChecker(player, -pos, size, Direction.LEFT);

        pos = new Vector2(0, distance);
        size = new Vector2(PLAYER_SIZE - SPARE_LENGTH, CHECK_COLLIDER_RANGE);
        GenerateContactChecker(player, pos, size, Direction.UP);
        GenerateContactChecker(player, -pos, size, Direction.DOWN);

    }

    //* It make gameObject that will check state(associate the wall)
    private static GameObject GenerateContactChecker(GameObject player, Vector2 pos, Vector2 size, Direction dir) {

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
            .SetDirection(dir);

        return checker;
    }
}