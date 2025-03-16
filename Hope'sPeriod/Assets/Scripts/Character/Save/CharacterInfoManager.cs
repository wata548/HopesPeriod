using System.Collections.Generic;

public static class CharacterInfoManager {

    public static List<EachCharacterInfo> Characters { get; private set; } = new();

    public static void Clear()
        => Characters.Clear();
    public static void Add(EachCharacterInfo info)
        => Characters.Add(info);
}