using QFramework;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SkateGame
{
    // 玩家落地事件
    public struct PlayerLandedEvent
    {
    }
    
    // 技巧执行事件
    public struct TrickPerformedEvent
    {
        public string TrickName;
    }
    
    // 技巧完成事件
    public struct TrickCompletedEvent
    {
    }
    
    // 状态切换事件
    public struct StateChangedEvent
    {
        public string FromState;
        public string ToState;
    }
    
    // 分数更新事件
    public struct ScoreUpdatedEvent
    {
        public int NewScore;
    }
    
    // 跳跃输入事件
    public struct JumpInputEvent
    {
    }
    
    // 轨道输入事件
    public struct GrindInputEvent
    {
    }
    
    // 技巧A输入事件
    public struct TrickAInputEvent
    {
    }
    
    // 技巧B输入事件
    public struct TrickBInputEvent
    {
    }
    
    // 强力轨道输入事件
    public struct PowerGrindInputEvent
    {
    }
    
    // 反向输入事件
    public struct ReverseInputEvent
    {
    }
    
    // 跳跃执行事件
    public struct JumpExecuteEvent
    {
    }
    
    // 移动输入事件
    public struct MoveInputEvent
    {
        public float HorizontalInput;
    }
    
    // UI显示相关事件
    public enum ScoreDisplayType
    {
        TotalScore,
        ComboScore,
        TrickScore
    }
    
    // 分数显示事件
    public struct ScoreDisplayEvent
    {
        public ScoreDisplayType ScoreType;
        public int Value;
        public Text TextComponent;
    }
    
    // 技巧显示事件
    public struct TrickDisplayEvent
    {
        public string TrickName;
        public Text TextComponent;
    }
    
    // 技巧列表显示事件
    public struct TrickListDisplayEvent
    {
        public List<TrickInfo> Tricks;
        public Text TextComponent;
    }
    
    // 连击显示事件
    public struct ComboDisplayEvent
    {
        public int ComboCount;
        public Text TextComponent;
    }
    
    // UI清理事件
    public struct UIClearEvent
    {
        public UIClearType ClearType;
    }
    
    public enum UIClearType
    {
        All,
        TrickList,
        Score,
        Notification
    }
}
