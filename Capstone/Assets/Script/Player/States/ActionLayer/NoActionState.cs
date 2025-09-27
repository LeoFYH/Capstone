public class NoActionState : ActionStateBase
{
    public override string GetStateName() => "None";
    public NoActionState()
    {
        isIgnoringMovementLayer = false;
        isRecovering = true;
    }
}
