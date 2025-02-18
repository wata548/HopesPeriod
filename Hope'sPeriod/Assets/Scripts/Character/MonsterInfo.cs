using System.Collections.Generic;

public static class MonsterInfo {

    public static Dictionary<int, int> Monsters { get; private set; } = new();

    public static void KillMonster(int code) {
        if (!Monsters.TryAdd(code, 1)) {
            Monsters[code]++;
        }
    }

    public static void Load(int code, int count) {
        Monsters.Add(code, count);
    }

    public static bool IsKill(int code, int count) {
        if (!Monsters.ContainsKey(code))
            return false;

        return Monsters[code] >= count;
    }
}