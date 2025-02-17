using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScripting: MonoBehaviour {
    [field: SerializeField] public Image Box { get; private set; }
    [field: SerializeField] public TMP_Text Name { get; private set; }
    [field: SerializeField] public TMP_Text Context { get; private set; }
    private const float Interval = 0.1f;

    private bool end = false;
    public bool End {
        get {
            bool temp = end;
            end = false;
            return temp;
        }
        private set {
            end = value;
        }
    }

    public void Erase() {
        Name.text = "";
        Context.text = "";
    }

    private bool start = false;
    private void Awake() {
        start = true;
        ShowScript(5002, $"이 {"???".AddEffect(Effect.Flow).AddColor(Color.red).SetSize(1.5f)}는 굉장히 중요한 물건이야{"!!!".AddEffect(Effect.Shake)}");
    }

    private int updateCount = 0;
    private void Update() {
        updateCount++;
        if (updateCount % 2 == 0) {
            updateCount = 0;
            Context.EffectProcedure();
        }
    }

    public void ShowScript(int actor, string context) {

        Context.text = "";
        Name.text = ActorInfo.Name(actor);
        StartCoroutine(Context.Typing(context, Interval, () => {
            End = true;
        }));
    }
    
}