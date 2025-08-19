using UnityEngine;
using SkateGame;

public class IdleState : StateBase
{
    private InputController player;
    private Rigidbody2D rb;

    public IdleState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Idle";

    public override void Enter()
    {
        // Debug.Log("进入Idle状态");
    }

    public override void Update()
    {
        // Idle状态下的逻辑
    }

    public override void Exit()
    {
        // Debug.Log("退出Idle状态");
    }
} 