using UnityEngine;

public class IdleState : StateBase
{
    public override string GetStateName() => "Idle";

    public override void Enter()
    {
        Debug.Log("进入Idle状态");
    }

    public override void Update()
    {
        // Idle状态下的逻辑
    }

    public override void Exit()
    {
        Debug.Log("退出Idle状态");
    }
} 