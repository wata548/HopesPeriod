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
        return FindEventInfo.FindItemInfos[MapCode].Contains(pos);
    }
}
sealed public class IsEventShowedScriptCommand : BooleanCommandBase {
    
    public int code { get; protected set; }
    public override bool Not { get; protected set; }
    protected override bool SimpleResult() => FindEventInfo.AlreadyEvent(code);
}
sealed public class IsHaveItemScriptCommand : BooleanCommandBase {
    
    public int Code { get; protected set; } 
    public int Count { get; protected set; }
    public override bool Not { get; protected set; }
    protected override bool SimpleResult() => Inventory.Items[Code] >= Count;
}
