using UnityEngine;

public abstract class BooleanCommandBase : ICommand {
    
    public abstract bool Not { get; protected set; }
    protected abstract bool SimpleResult();

    public bool Result() {

        bool result = SimpleResult();
        if (Not)
            return !result;
        return result;
    } 
} 


sealed public class IsUseItemScriptCommand : BooleanCommandBase {
    public int Code { get; protected set; }
    public int Count { get; protected set; }


    public override bool Not { get; protected set; }

    protected override bool SimpleResult() {
        Inventory.Use.TryGetValue(Code, out int value);
        return value >= Count;
    }
}
sealed public class IsFindItemScriptCommand : BooleanCommandBase {
    public int MapCode { get; protected set; }
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public int Z { get; protected set; }

    public override bool Not { get; protected set; }

    protected override bool SimpleResult() {
        var pos = new Vector3Int(X, Y, Z);
        if (!AlreadyFindEventInfo.FindItemInfos.ContainsKey(MapCode))
            return false;
        
        return AlreadyFindEventInfo.FindItemInfos[MapCode].Contains(pos);
    }
}
sealed public class IsEventShowedScriptCommand : BooleanCommandBase {
    
    public int Code { get; protected set; }
    public override bool Not { get; protected set; }
    protected override bool SimpleResult() => AlreadyFindEventInfo.AlreadyEvent(Code);
}
sealed public class IsHaveItemScriptCommand : BooleanCommandBase {
    
    public int Code { get; protected set; } 
    public int Count { get; protected set; }
    public override bool Not { get; protected set; }

    protected override bool SimpleResult() {

        if (!Inventory.Items.ContainsKey(Code)) {
            return false;
        }

        return Inventory.Items[Code] >= Count;
    }
}
sealed public class IsChapterEqualScriptCommand : BooleanCommandBase {
    
    public int Chapter { get; protected set; }
    public override bool Not { get; protected set; }

    protected override bool SimpleResult() => ChapterInfo.Chapter == Chapter;
}

sealed public class IsKillMonsterScriptCommand : BooleanCommandBase {
    public override bool Not { get; protected set; }
    public int Code { get; protected set; }
    public int Count { get; protected set; }
    protected override bool SimpleResult() => (MonsterInfo.IsKill(Code, Count));
}