
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DefaultAndEnumDataParser))]
public class DefulatAndEnumSyncEditor: Editor {

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        GUILayout.Label("It make directory and EnumType (please press first and wait for loading)");
        if (GUILayout.Button("SetUp")) {
            
            var parser = target as DefaultAndEnumDataParser;
            parser.SetUp();
        }
        
        GUILayout.Label("It make data type (please press second and wait for loading)");
        if (GUILayout.Button("GenerateData")) {
            var parser = target as DefaultAndEnumDataParser;

            foreach (var factor in parser.Path) {

                parser.Generate(factor);
            }
        }
        
        if (GUILayout.Button("SyncData")) {
            var parser = target as DefaultAndEnumDataParser;

            foreach (var factor in parser.Path) {
                
                parser.Sync(factor);
            }
        }
    }

}


[CustomEditor(typeof(MultiTypeParser))]
public class MultiTypeSyncEditor: Editor {

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        GUILayout.Label("It make directory(please press first and wait for loading)");
        if (GUILayout.Button("SetUp")) {
            
            var parser = target as MultiTypeParser;
            parser.SetUp();
        }
        
        GUILayout.Label("It make data type (please press second and wait for loading)");
        if (GUILayout.Button("GenerateData")) {
            var parser = target as MultiTypeParser;
            parser.Generate(parser.Path[0]);
        }
        
        if (GUILayout.Button("SyncData")) {
            var parser = target as MultiTypeParser;
            parser.Sync(parser.Path);
        }
    }

}
#endif