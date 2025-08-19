using UnityEngine;
using SkateGame;

public class TrickB : TrickBase
{
    public TrickB()
    {
        trickName = "TrickB";
        duration = 2.0f;
        scoreValue = 150;
    }

    public override void PerformTrick(InputController player)
    {
        // Debug.Log("执行技巧B");
        PlayAnimation(player);
        PlayEffects(player);
        AddScore();
    }

    public override void Exit(InputController player)
    {
        // Debug.Log("退出技巧B");
    }

    protected override void PlayAnimation(InputController player)
    {
        // Debug.Log("播放技巧B动画");
    }

    protected override void PlayEffects(InputController player)
    {
        // Debug.Log("播放技巧B特效");
    }

    protected override void AddScore()
    {
        TrickScore.Instance.AddTrickScore(this);
    }
} 