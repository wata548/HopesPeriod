using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Serialization;

public abstract class RawDataLoader: ScriptableObject {
    
    public abstract List<List<string>> Load(string path);
}
public abstract class RawListDataLoader: ScriptableObject { 
    
    [FormerlySerializedAs("usingFirstRawToKey")] [SerializeField] public bool UsingFirstRawToKey = true;
    
    public abstract List<List<List<string>>> Load(string path);
}
