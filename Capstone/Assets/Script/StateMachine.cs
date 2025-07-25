using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// 状态机类
public class E
{
    // 当前状态
    private StateBase currentState;
    
    // 状态字典，用于存储所有状态
    private Dictionary<string, StateBase> states = new Dictionary<string, StateBase>();
    
    // 状态切换事件
    public event Action<StateBase, StateBase> OnStateChanged; // 参数：旧状态，新状态
    
    // 构造函数
    public E()
    {
        currentState = null;
    }
    
    // 添加状态
    public void AddState(string stateName, StateBase state)
    {
        if (!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }
    
    // 切换到指定状态（无参数）
    public void SwitchState(string stateName)
    {
        SwitchState(stateName, null);
    }
    
    // 切换到指定状态（带参数）
    public void SwitchState(string stateName, object data)
    {
        if (states.ContainsKey(stateName))
        {
            StateBase oldState = currentState;
            
            // 退出当前状态
            if (currentState != null)
            {
                currentState.Exit();
            }
            
            // 切换到新状态
            currentState = states[stateName];
            
            // 进入新状态（传递数据）
            if (currentState is TrickState trickState && data is string trickName)
            {
                //调用trickstate的settrickname方法来传入特技名称
                trickState.SetTrickName(trickName);
            }
            currentState.Enter();
            
            // 触发状态切换事件
            OnStateChanged?.Invoke(oldState, currentState);
        }
        else
        {
            // Debug.LogWarning($"状态 '{stateName}' 不存在！");
        }
    }
    
    // 获取当前状态
    public StateBase GetCurrentState()
    {
        return currentState;
    }
    
    // 获取状态名称
    public string GetCurrentStateName()
    {
        if (currentState != null)
        {
            return currentState.GetStateName();
        }
        return "Unknown";
    }
    
    // 检查当前状态是否为指定状态
    public bool IsCurrentState(string stateName)
    {
        return states.ContainsKey(stateName) && states[stateName] == currentState;
    }
    
    // 获取所有状态名称
    public string[] GetAllStateNames()
    {
        return states.Keys.ToArray();
    }
    
    // 更新当前状态
    public void UpdateCurrentState()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
    
    // 清除所有状态
    public void ClearStates()
    {
        states.Clear();
        currentState = null;
    }
} 