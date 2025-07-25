using UnityEngine;

public abstract class TrickBase
{
    public string trickName;
    public float duration;
    public int scoreValue;

    public virtual void PerformTrick(PlayerScript player)
    {
        Debug.Log($"执行技巧: {trickName}");
    }

    public virtual void Exit(PlayerScript player)
    {
        
    }
    //在子类trickbase中分别写
    protected abstract void PlayAnimation(PlayerScript player);
    protected abstract void PlayEffects(PlayerScript player);
    protected abstract void AddScore();
} 