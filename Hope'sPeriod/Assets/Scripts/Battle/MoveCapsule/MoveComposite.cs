using System;using UnityEngine;

public abstract class MoveComposite: IComparable<MoveComposite> {

    public abstract int Priority { get; protected set; }
    public abstract Direction Apply { get; set; }
    public abstract float Power { get; set; }

    public abstract GameObject Owner { get; protected set; }

    public MoveComposite SetOwner(GameObject owner) {
        Owner = owner;

        return this;
    }

    public MoveComposite(GameObject owner) => Owner = owner;

    public abstract Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo = Direction.None);

    public int CompareTo(MoveComposite other) {

        return Priority.CompareTo(other.Priority);
    }
}