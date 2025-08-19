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
        private InputController playerController;
        
        protected override void OnInit()
        {
            // 获取玩家控制器
            playerController = Object.FindFirstObjectByType<InputController>();
            
            // 监听输入事件
            this.RegisterEvent<TrickAInputEvent>(OnTrickAInput);
            this.RegisterEvent<TrickBInputEvent>(OnTrickBInput);
            this.RegisterEvent<JumpInputEvent>(OnJumpInput);
            this.RegisterEvent<GrindInputEvent>(OnGrindInput);
            this.RegisterEvent<PowerGrindInputEvent>(OnPowerGrindInput);
            this.RegisterEvent<ReverseInputEvent>(OnReverseInput);
            
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
            if (playerController != null && !playerController.IsGrounded())
            {
                // 直接切换到技巧状态，传递技巧名称
                playerController.stateMachine.SwitchState("Trick", "TrickA");
                Debug.Log("处理技巧A输入");
            }
        }

        private void OnTrickBInput(TrickBInputEvent evt)
        {
            if (playerController != null && !playerController.IsGrounded())
            {
                // 直接切换到技巧状态，传递技巧名称
                playerController.stateMachine.SwitchState("Trick", "TrickB");
                Debug.Log("处理技巧B输入");
            }
        }

        private void OnJumpInput(JumpInputEvent evt)
        {
            if (playerController != null && playerController.IsGrounded())
            {
                playerController.stateMachine.SwitchState("Jump");
                Debug.Log("处理跳跃输入");
            }
        }

        private void OnGrindInput(GrindInputEvent evt)
        {
            if (playerController != null && !playerController.IsGrounded())
            {
                if (playerController.isNearTrack && playerController.grindJumpTimer <= 0f)
                {
                    playerController.stateMachine.SwitchState("Grind");
                    Debug.Log("切换到轨道状态");
                }
                else if (playerController.isNearWall)
                {
                    playerController.stateMachine.SwitchState("WallRide");
                    Debug.Log("切换到墙壁骑行状态");
                }
                else
                {
                    playerController.stateMachine.SwitchState("Grab");
                    Debug.Log("切换到抓取状态");
                }
            }
        }
        
        private void OnPowerGrindInput(PowerGrindInputEvent evt)
        {
            if (playerController != null && playerController.IsGrounded())
            {
                playerController.stateMachine.SwitchState("PowerGrind");
                Debug.Log("切换到强力轨道状态");
            }
        }
        
        private void OnReverseInput(ReverseInputEvent evt)
        {
            if (playerController != null && playerController.IsGrounded())
            {
                playerController.stateMachine.SwitchState("Reverse");
                Debug.Log("切换到反向状态");
            }
        }

        private void OnStateChanged(StateChangedEvent evt)
        {
            Debug.Log($"状态切换: {evt.FromState} -> {evt.ToState}");
        }
    }
}
