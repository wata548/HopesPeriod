using System.Runtime.InteropServices.WindowsRuntime;
using SpreadInfo;
using UnityEngine;

public static class ActorInfo {

    private static ActorDBDataTable info = null;

    private static void SetTable() {
        if (info is not null)
            return;
        info = Resources.Load<ActorDBDataTable>("SpreadInfo/Generated/ActorDBDataTable");
    }

    public static string Name(int code) {
        SetTable();
        return info.DataTable[code].Name;
    }  
}