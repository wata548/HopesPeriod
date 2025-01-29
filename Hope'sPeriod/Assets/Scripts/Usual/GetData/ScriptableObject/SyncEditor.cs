using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(DefaultAndEnumDataParser))]
public class SyncEditor: Editor {

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        GUILayout.Label("It make directory and EnumType (please press first and wait for loading)");
        if (GUILayout.Button("SetUp")) {
            
            DefaultAndEnumDataParser parser = target as DefaultAndEnumDataParser;
            parser.SetUp();
        }
        
        GUILayout.Label("It make data type (please press second and wait for loading)");
        if (GUILayout.Button("GenerateData")) {
            DefaultAndEnumDataParser parser = target as DefaultAndEnumDataParser;

            foreach (var factor in parser.Path) {

                parser.Generate(factor);
            }
        }
        
        if (GUILayout.Button("SyncData")) {
            DefaultAndEnumDataParser parser = target as DefaultAndEnumDataParser;

            foreach (var factor in parser.Path) {
                
                parser.Sync(factor);
            }
        }
    }

}
#endif