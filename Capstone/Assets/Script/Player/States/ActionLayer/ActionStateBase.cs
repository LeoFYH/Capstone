using Unity.VisualScripting;
using UnityEngine;

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
    public sealed override void Update()
    {
        if (!isLoop)
        {
            StateTimeUpdate();
            CheckIgnoreMovementLayer();
            CheckRecovering();
        }
        UpdateActionState();
    }

    private void StateTimeUpdate()
    {
        if(stateTimer <= stateTotalDuration)
        {
            stateTimer += Time.deltaTime;
        }
        else
        {
            Exit();
        }
    }
    private void CheckIgnoreMovementLayer()
    {
        if(stateTimer > ignoreMovementLayerDuration.x && stateTimer < ignoreMovementLayerDuration.y)
        {
            isIgnoringMovementLayer = true;
        }
    }
    private void CheckRecovering()
    {
        if(stateTimer > recoveryDuration.x && stateTimer < recoveryDuration.y)
        {
            isRecovering = true;
        }
    }
}