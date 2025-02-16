using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScripting: MonoBehaviour {
    [field: SerializeField] public Image Box { get; private set; }
    [field: SerializeField] public TMP_Text Name { get; private set; }
    [field: SerializeField] public TMP_Text Context { get; private set; }
}