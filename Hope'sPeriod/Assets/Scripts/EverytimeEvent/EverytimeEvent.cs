using SpreadInfo;
using UnityEngine;

public static class EverytimeEvent {
    private static EverytimeEventDBDataTable dataTable = null;
    
    private static void SetTable() {
        dataTable ??= Resources.Load<EverytimeEventDBDataTable>("SpreadInfo/Generated/EverytimeEventDBDataTable");
    }

    public static int StartEvent() {
        SetTable();

        foreach (var factor in dataTable.DataTable) {
            bool available = ScriptCodeInterpreter
                .Interpret(factor.Value.Condition)
                .ToCondition();

            if (!available)
                continue;

            int code = factor.Value.Target;
            FindEventInfo.FindEvent(factor.Key);
            Debug.Log(factor.Key);
            
            
            switch (code.ToCodeType()) {
                case CodeType.Script :
                    ScriptShower.Instance.StartScript(code);
                    break;
                case CodeType.Tutorial:
                    ScriptShower.Instance.ShowTutorial(code);
                    break;
            }

            break;
        }

        return 0;
    }
}