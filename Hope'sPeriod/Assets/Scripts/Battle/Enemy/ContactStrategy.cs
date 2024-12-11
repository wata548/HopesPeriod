using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public abstract class ContactStrategy {
    
    private Action<GameObject> process = null;

    public void SetProcess(Action<GameObject> process) {
        
        this.process = process ?? throw new Exception("It is not process Function");
    }

    public virtual void TriggerContact(GameObject target) {

        process?.Invoke(target);
    }
    
    public virtual void ColliderContact(GameObject target) {
    
        process?.Invoke(target);
    }

    protected ContactStrategy(Action<GameObject> process) => this.process = process;
}

public class TriggerType : ContactStrategy {

    public TriggerType(Action<GameObject> process = null): base(process) {}
    public override void ColliderContact(GameObject target) {}
}

public class ColliderType : ContactStrategy {

    public ColliderType(Action<GameObject> process = null) : base(process) {}
    public override void TriggerContact(GameObject target) {}

}