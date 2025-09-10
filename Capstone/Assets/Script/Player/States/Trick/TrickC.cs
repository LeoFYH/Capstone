using UnityEngine;
using SkateGame;

public class TrickC : TrickBase
{
    public TrickC()
    {
        trickName = "TrickC";
        duration = 2.5f;
        scoreValue = 200;
    }

    public override void PerformTrick(InputController player)
    {
        Debug.Log("执行技巧C");
        PlayAnimation(player);
        PlayEffects(player);
        // 不再在这里调用AddScore，分数由系统层处理
    }

    public override void Exit(InputController player)
    {
        Debug.Log("退出技巧C");
    }

    protected override void PlayAnimation(InputController player)
    {
        Debug.Log("播放技巧C动画");
    }

    protected override void PlayEffects(InputController player)
    {
        Debug.Log("播放技巧C特效");
    }

    protected override void AddScore()
    {
        TrickScore.Instance.AddTrickScore(this);
    }
}
