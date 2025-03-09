using System;

public class ContactInfo {

    public Direction Contact { get; private set; }= Direction.None;

    public void ContactIn(Direction dir) {

        Contact |= dir;
    }

    public void ContactOut(Direction dir) {
        if ((Contact & dir) == 0)
            return;
        Contact ^= dir;
    }
}
