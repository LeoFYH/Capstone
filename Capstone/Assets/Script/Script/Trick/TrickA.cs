using UnityEngine;
using SkateGame;

public class TrickA : TrickBase
{
    public TrickA()
    {
        trickName = "TrickA";
        duration = 1.5f;
        scoreValue = 20;
    }

    public override void PerformTrick(InputController player)
    {
        Debug.Log("执行技巧A");
        PlayAnimation(player);
        PlayEffects(player);
        
        // 改变玩家颜色为红色
        player.ChangePlayerColor(Color.red);
        
        // 不再在这里调用AddScore，分数由系统层处理
    }

    public override void Exit(InputController player)
    {
        Debug.Log("退出技巧A");
        // 恢复玩家颜色
        player.ResetPlayerColor();
    }

    protected override void PlayAnimation(InputController player)
    {
        Debug.Log("播放技巧A动画");
    }

    protected override void PlayEffects(InputController player)
    {
        Debug.Log("播放技巧A特效");
    }

    protected override void AddScore()
    {
        TrickScore.Instance.AddTrickScore(this);
    }
} 