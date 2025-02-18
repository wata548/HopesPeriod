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

            if (available) {

                Debug.Log($"{factor.Key}");
            }
        }

        return 0;
    }
}