using UnityEngine;

public class TrickState : StateBase
{
    private Rigidbody2D rb;
    private PlayerScript player;
    private float TrickTime;
    private float TrickTimeMax = 1.5f;
    private float comboPoint;
    public TrickState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Trick";

    public override void Enter()
    {
        player.canBeHurt = true;
        TrickTime = TrickTimeMax;
    }

    public override void Update()
    {
        TrickTime -= Time.deltaTime;
        if (TrickTime <= 0)
        { 
           /////////////Need a state///////////
        }
    }

    public override void Exit()
    {
        player.canBeHurt = false;
    }
}
