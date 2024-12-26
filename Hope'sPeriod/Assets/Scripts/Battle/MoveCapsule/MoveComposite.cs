using System;using UnityEngine;

public abstract class MoveComposite: IComparable<MoveComposite> {

    public abstract int Priority { get; protected set; }
    public abstract Direction Apply { get; set; }
    public abstract float Power { get; set; }

    public abstract Vector2 Play(Vector2 beforeVelo, Vector2 currentVelo);

    public int CompareTo(MoveComposite other) {

        return Priority.CompareTo(other.Priority);
    }
}