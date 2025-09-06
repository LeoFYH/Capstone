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
        
        // 检测等级，如果等级是A以上，奖励玩家一个跳跃
        CheckGradeAndRewardJump(player);
        
        // 不再在这里调用AddScore，分数由系统层处理
    }
    
    private void CheckGradeAndRewardJump(InputController player)
    {
        // 通过GameApp获取当前等级
        var scoreModel = GameApp.Interface.GetModel<IScoreModel>();
        if (scoreModel != null)
        {
            char currentGrade = scoreModel.CurrentGrade.Value;
            Debug.Log($"TrickA: 当前等级为 {currentGrade}");
            
            // 如果等级是A或S，奖励跳跃
            if (currentGrade == 'A' || currentGrade == 'S')
            {
                Debug.Log($"TrickA: 等级 {currentGrade} 达到奖励条件，给予跳跃奖励！");
                player.RewardJump();
            }
            else
            {
                Debug.Log($"TrickA: 等级 {currentGrade} 未达到奖励条件（需要A级以上）");
            }
        }
        else
        {
            Debug.LogWarning("TrickA: 无法获取ScoreModel，跳过等级检测");
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