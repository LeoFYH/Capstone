using UnityEngine;
using SkateGame;

public class TrickA : TrickBase
{
    public TrickA()
    {
        trickName = "TrickA";
        duration = 1.5f;
        scoreValue = 100;
    }

    public override void PerformTrick(InputController player)
    {
        // Debug.Log("执行技巧A");
        PlayAnimation(player);
        PlayEffects(player);
        AddScore();
    }

    public override void Exit(InputController player)
    {
        // Debug.Log("退出技巧A");
    }

    protected override void PlayAnimation(InputController player)
    {
        // Debug.Log("播放技巧A动画");
    }

    protected override void PlayEffects(InputController player)
    {
        // Debug.Log("播放技巧A特效");
    }

    protected override void AddScore()
    {
        TrickScore.Instance.AddTrickScore(this);
    }
} 