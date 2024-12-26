public sealed class CompoDefault : CompositeBase {

    public CompoDefault() {

        AddComposite(new CompoInput());
        AddComposite(new CompoGravity());
        AddComposite(new CompoFriction());
    }
}