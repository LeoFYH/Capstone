using UnityEngine;
using SkateGame;
using QFramework;

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
        ///
        /// 
    }

    public override void Update()
    {
        // Idle状态下不处理移动，保持静止
        // 移动输入由InputController检测并切换到Move状态
    }

    public override void Exit()
    {
        // Debug.Log("退出Idle状态");
    }
} 