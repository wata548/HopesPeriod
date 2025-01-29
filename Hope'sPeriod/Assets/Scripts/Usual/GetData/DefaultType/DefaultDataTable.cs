using UnityEngine;
using System.Collections.Generic;

public class DefaultDataTable<T>: ScriptableObject where T : DefaultDataType {

    [SerializeField] protected SerializableDictionary<int, T> m_DataTable = new();
    public SerializableDictionary<int, T> DataTable => m_DataTable;
}

public class ListTypeDataTable<T> : ScriptableObject where T : DefaultDataType {

    [SerializeField] protected SerializableDictionary<int, List<T>> m_DataTable = new();
    public SerializableDictionary<int, List<T>> DataTable => m_DataTable;
}