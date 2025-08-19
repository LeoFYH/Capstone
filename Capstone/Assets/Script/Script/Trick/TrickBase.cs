using UnityEngine;
using SkateGame;

public abstract class TrickBase
{
    public string trickName;
    public float duration;
    public int scoreValue;

    public virtual void PerformTrick(InputController player)
    {
        Debug.Log($"执行技巧: {trickName}");
    }

    public virtual void Exit(InputController player)
    {
        
    }
    //在子类trickbase中分别写
    protected abstract void PlayAnimation(InputController player);
    protected abstract void PlayEffects(InputController player);
    protected abstract void AddScore();
} 