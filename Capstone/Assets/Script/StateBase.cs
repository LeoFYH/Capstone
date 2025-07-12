using UnityEngine;

// 状态基类
public abstract class StateBase
{
    // 进入状态时调用
    public virtual void Enter() { }
    
    // 状态更新时调用
    public virtual void Update() { }
    
    // 退出状态时调用
    public virtual void Exit() { }
    
    // 获取状态名称
    public abstract string GetStateName();
} 