using UnityEngine;
using SkateGame;
using QFramework;

public abstract class TrickBase: ICanGetModel, IBelongToArchitecture
{
    public IPlayerModel playerModel;
    public string trickName;
    public float duration;
    public int scoreValue;

    protected TrickBase()
    {
        playerModel = this.GetModel<IPlayerModel>();
    }

    public virtual void PerformTrick(InputController player)
    {
        Debug.Log($"执行技巧: {trickName}");
    }

    public virtual void Exit(InputController player)
    {
        
    }
    
    public IArchitecture GetArchitecture() => SkateGame.GameApp.Interface;
    //在子类trickbase中分别写
    protected abstract void PlayAnimation(InputController player);
    protected abstract void PlayEffects(InputController player);
    protected abstract void AddScore();
} 