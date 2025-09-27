using UnityEngine;
using QFramework;
using SkateGame;

// 状态基类
public abstract class StateBase : ICanGetModel, IBelongToArchitecture
{
    protected InputController player;
    protected Rigidbody2D rb;
    protected IPlayerModel playerModel;
    protected IInputModel inputModel;

    protected StateBase()
    {
        playerModel = this.GetModel<IPlayerModel>();
        inputModel = this.GetModel<IInputModel>();
    }
    // 进入状态时调用
    public virtual void Enter() { }
    
    // 状态更新时调用
    public virtual void Update() { }
    
    // 退出状态时调用
    public virtual void Exit() { }
    
    // 获取状态名称
    public abstract string GetStateName();

    public IArchitecture GetArchitecture() => SkateGame.GameApp.Interface;
    
    // 发送事件的辅助方法（通过InputController）
    protected void SendEvent<T>(T evt) where T : struct
    {
        // 这里需要通过InputController来发送事件
        // 暂时留空，在具体状态中实现
    }
} 