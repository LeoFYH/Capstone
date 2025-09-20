using UnityEngine;
using SkateGame;

public class TrickA : TrickBase
{
    public TrickA()
    {
        trickName = "TrickA";
        duration = playerModel.TrickADuration.Value;
        scoreValue = playerModel.TrickAScore.Value;
    }

    public override void PerformTrick(InputController player)
    {
        Debug.Log("执行技巧A");
        PlayAnimation(player);
        PlayEffects(player);
        
        // 改变玩家颜色为红色
        player.ChangePlayerColor(Color.red);
        
        // 检测是否在能量状态，如果是则给予奖励
        CheckIfInPower(player);
        
        // 不再在这里调用AddScore，分数由系统层处理
    }
    
    private void CheckIfInPower(InputController player)
    {
        // 通过GameApp获取PlayerModel
        var playerModel = GameApp.Interface.GetModel<IPlayerModel>();
        if (playerModel != null)
        {
            // 检查是否在能量状态
            if (playerModel.isInPower.Value)
            {
                Debug.Log("TrickA: 检测到能量状态，给予跳跃奖励！");
                player.RewardJump();
                playerModel.isInPower.Value = false; // 消耗能量状态
            }
            else
            {
                Debug.Log("TrickA: 不在能量状态，无奖励");
            }
        }
        else
        {
            Debug.LogWarning("TrickA: 无法获取PlayerModel，跳过能量检测");
        }
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