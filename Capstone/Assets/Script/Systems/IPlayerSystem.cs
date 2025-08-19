using QFramework;
using UnityEngine;

namespace SkateGame
{
    public interface IPlayerSystem : ISystem
    {
        void UpdatePlayerState();
        void UpdateAirTime(float deltaTime);
        void CheckGrounded();
    }

    public class PlayerSystem : AbstractSystem, IPlayerSystem, ICanSendCommand
    {
        protected override void OnInit()
        {
            // 监听输入事件
            this.RegisterEvent<TrickAInputEvent>(OnTrickAInput);
            this.RegisterEvent<TrickBInputEvent>(OnTrickBInput);
            this.RegisterEvent<JumpInputEvent>(OnJumpInput);
            this.RegisterEvent<GrindInputEvent>(OnGrindInput);
            
            // 监听状态切换事件
            this.RegisterEvent<StateChangedEvent>(OnStateChanged);
        }

        public void UpdatePlayerState()
        {
            var playerModel = this.GetModel<IPlayerModel>();
            
            // 更新玩家状态逻辑
            if (playerModel.IsInAir.Value)
            {
                playerModel.AirTime.Value += Time.deltaTime;
            }
            else
            {
                playerModel.AirTime.Value = 0f;
            }
        }

        public void UpdateAirTime(float deltaTime)
        {
            var playerModel = this.GetModel<IPlayerModel>();
            
            if (playerModel.IsInAir.Value)
            {
                playerModel.AirTime.Value += deltaTime;
            }
        }



        public void CheckGrounded()
        {
            // 检查玩家是否着地的逻辑
            // 这里需要根据实际的游戏逻辑来实现
            // 例如：射线检测、碰撞检测等
            var playerModel = this.GetModel<IPlayerModel>();
            
            // 示例：简单的着地检测逻辑
            // 实际项目中需要根据具体的物理系统来实现
            // playerModel.IsGrounded.Value = Physics2D.Raycast(...);
        }

        // 输入事件处理
        private void OnTrickAInput(TrickAInputEvent evt)
        {
            this.SendCommand(new PerformTrickCommand { TrickName = "TrickA" });
            Debug.Log("处理技巧A输入");
        }

        private void OnTrickBInput(TrickBInputEvent evt)
        {
            this.SendCommand(new PerformTrickCommand { TrickName = "TrickB" });
            Debug.Log("处理技巧B输入");
        }

        private void OnJumpInput(JumpInputEvent evt)
        {
            // 处理跳跃逻辑
            Debug.Log("处理跳跃输入");
        }

        private void OnGrindInput(GrindInputEvent evt)
        {
            // 处理轨道逻辑
            Debug.Log("处理轨道输入");
        }

        private void OnStateChanged(StateChangedEvent evt)
        {
            Debug.Log($"状态切换: {evt.FromState} -> {evt.ToState}");
        }
    }
}
