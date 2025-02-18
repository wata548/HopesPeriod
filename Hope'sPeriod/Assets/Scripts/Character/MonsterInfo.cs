using System.Collections.Generic;

public static class MonsterInfo {

    public static Dictionary<int, int> Monsters { get; private set; } = new();

    private static void Clear() {
        Monsters.Clear();
    }
    
    public static void KillMonster(int code) {
        if (!Monsters.TryAdd(code, 1)) {
            Monsters[code]++;
        }
    }

    public static void Load(SaveMonster[] datas) {
        Clear();
        
        foreach (var data in datas) {
            Monsters.Add(data.Code, data.KillCount);
        }
    }

    public static bool IsKill(int code, int count) {
        if (!Monsters.ContainsKey(code))
            return false;

        return Monsters[code] >= count;
    }
}