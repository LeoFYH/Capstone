using QFramework;

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
}
