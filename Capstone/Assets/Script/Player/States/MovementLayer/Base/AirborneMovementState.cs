using SkateGame;
public abstract class AirborneMovementState : StateBase
{
    protected virtual void UpdateAirMovement(){}
    
    public sealed override void Update()
    {
        UpdateAirMovement();
    }
}


