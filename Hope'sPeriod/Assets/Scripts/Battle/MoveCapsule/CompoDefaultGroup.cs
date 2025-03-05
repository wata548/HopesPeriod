using UnityEngine;

public sealed class CompoDefaultGroup : CompositeGroupBase {

    public CompoDefaultGroup(GameObject owner = null) : base(owner) {

        AddComposite(new CompoInput(owner));
        AddComposite(new CompoGravity(owner));
        AddComposite(new CompoFriction(owner));
    }
}