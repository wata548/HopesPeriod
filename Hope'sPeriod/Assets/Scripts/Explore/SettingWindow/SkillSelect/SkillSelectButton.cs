using UnityEngine;

public class SkillSelectButton: InteractButtonUI {
    private const float CharacterInterval = -275;
    private static readonly Vector2 SkillInterval = new(400, -94);
    private static readonly Vector2 DefaultPos = new(0, 0);
    
    public override void Click() {

        int skillLimit = EachCharacterInfo.SkillCountLimit;
        int character = Index / skillLimit;
        int skillIndex = (Index % skillLimit) + 1;

        var pos = DefaultPos;
        pos.y += character * CharacterInterval;
        if ((skillIndex & 1) == 0)
            pos.y += SkillInterval.y;
        if (skillIndex >= 3)
            pos.x += SkillInterval.x;

        Parse(Manager).ShowList(pos, character);
    }

    private SkillSelectButtonManager Parse(InteractButtonManager manager) {
        if (manager is not SkillSelectButtonManager result)
            throw new TypeMissMatched(manager.gameObject, typeof(SkillSelectButtonManager));
        
        return result;
    }
}