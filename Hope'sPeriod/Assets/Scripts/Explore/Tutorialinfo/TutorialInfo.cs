using System;
using System.Collections.Generic;
using SpreadInfo;
using UnityEngine;

public static class TutorialInfo {

    private static TutorialDBDataTable table = null;

    private static void SetTable() {

        table ??= Resources.Load<TutorialDBDataTable>("SpreadInfo/Generated/TutorialDBDataTable");
    }

    public static List<TutorialScriptCommand> Interpret(int code) {

        SetTable();
        if (!table.DataTable.ContainsKey(code))
            throw new Exception($"This code is wrong: {code}");

        var list = ScriptCodeInterpreter.InterpretToCommandBase(table.DataTable[code].Info);
        List<TutorialScriptCommand> result = new();
        foreach (var element in list) {

            if (element.Item1 != ScriptCodeKeyword.Tutorial)
                throw new Exception("this command must be Tutorial Type");

            var value = element.Item2 as TutorialScriptCommand;
            result.Add(value);
        }

        return result;
    }
}