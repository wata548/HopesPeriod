/*

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


[CustomEditor(typeof(Slider))]
public class UpdateTestSlider : Editor {
    [SerializeField] private float ratio = 1f;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        ratio = EditorGUILayout.Slider("Raio", ratio, 0, 1);
        Slider slider = (Slider)target;
        
        if (GUILayout.Button("Update")) {
            
            slider.UpdateInfo(ratio);
        }
    }
}
*/