using Unity.VisualScripting;
using UnityEngine;
using SkateGame;

// Base class for action-layer states that may suppress movement
public abstract class ActionStateBase : StateBase
{
    protected float stateTimer;
    protected float stateTotalDuration = 0;
    protected bool isLoop = false;
    protected bool isIgnoringMovementLayer = true;
    protected bool isRecovering = false;
    protected Vector2 ignoreMovementLayerDuration; // 第一个参数是开始忽略的时间，第二个参数是结束忽略的时间
    protected Vector2 recoveryDuration; // 第一个参数是开始后摇的时间，第二个参数是结束后摇的时间
    public bool IsIgnoringMovementLayer => isIgnoringMovementLayer;
    public bool IsRecovering => isRecovering;
    protected virtual void UpdateActionState(){}
    protected virtual void EnterActionState(){}
    protected virtual void ExitActionState(){}
    protected ActionStateBase(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }
    public sealed override void Enter()
    {
        player.animator.SetLayerWeight(0, 0);
        player.animator.SetLayerWeight(1, 1);
        EnterActionState();
    }
    public sealed override void Update()
    {
        if (!isLoop)
        {
            StateTimeUpdate();
            CheckIgnoreMovementLayer();
            CheckRecovering();
        }
        if(isRecovering)
        {
            CheckSwitchAction();
        }
        UpdateActionState();
    }
    public sealed override void Exit()
    {
        player.animator.SetLayerWeight(0, 1);
        player.animator.SetLayerWeight(1, 0);
        ExitActionState();
    }

    private void StateTimeUpdate()
    {
        if(stateTimer <= stateTotalDuration)
        {
            stateTimer += Time.deltaTime;
        }
        else
        {
            player.stateMachine.SwitchState(StateLayer.Action, "None");
        }
    }
    
    // 检查是否忽略运动层
    private void CheckIgnoreMovementLayer()
    {
        if(stateTimer > ignoreMovementLayerDuration.x && stateTimer < ignoreMovementLayerDuration.y)
        {
            isIgnoringMovementLayer = true;
        }
    }

    // 检查是否后摇
    private void CheckRecovering()
    {
        if(stateTimer > recoveryDuration.x && stateTimer < recoveryDuration.y)
        {
            isRecovering = true;
        }
    }

    // 检查是否切换到其他状态
    private void CheckSwitchAction()
    {
        // 优先Trick
        if(inputModel.TrickStart.Value && !playerModel.IsGrounded.Value)
        {
            player.stateMachine.SwitchState(StateLayer.Action, "TrickA");
        }
        // 其次Grind
        else if (inputModel.Grind.Value)
        {
            GrindInput();
        }
    }
    private void GrindInput()
    {
        // 优先滑轨
        if (playerModel.GrindJumpTimer.Value <= 0f && playerModel.IsNearTrack.Value)
        {
            player.stateMachine.SwitchState(StateLayer.Action, "Grind");
        }
        // 其次滑墙
        else if (!playerModel.IsGrounded.Value)
        {
            if(playerModel.IsNearWall.Value)
            {
                player.stateMachine.SwitchState(StateLayer.Action, "WallRide");
            }
            // 最后Grab
            else
            {
                player.stateMachine.SwitchState(StateLayer.Action, "Grab");
            }
        }
    }
}