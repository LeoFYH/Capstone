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
        private PlayerController playerController;
        private IPlayerModel playerModel;
        
        protected override void OnInit()
        {
            // 获取玩家控制器
            playerController = Object.FindFirstObjectByType<PlayerController>();
            
            // 获取玩家参数
            playerModel = this.GetModel<IPlayerModel>();
            
            // 监听输入事件
            this.RegisterEvent<TrickAInputEvent>(OnTrickAInput);
            this.RegisterEvent<TrickBInputEvent>(OnTrickBInput);
            this.RegisterEvent<JumpExecuteEvent>(OnJumpExecute);
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
            Debug.Log($"PlayerSystem: 接收到TrickAInputEvent");
            Debug.Log($"  - playerController存在: {playerController != null}");
            if (playerController != null)
            {
                if (playerController.TrickAEffect != null)
                {
                    playerController.TrickAEffect.PlayFeedbacks();
                }

                // 使用 Raycast 检测 InteractiveLayer 对象
                DetectInteractiveObjectsWithRaycast();
            }
            
            if (playerController != null && !playerModel.IsGrounded.Value)
            {
                Debug.Log("PlayerSystem: 在空中执行TrickA");
                playerController.stateMachine.SwitchState(StateLayer.Action, "TrickA");
            }
            else
            {
                Debug.Log($"PlayerSystem: 无法执行技巧 - 在地面上或playerController为空");
            }
        }

        private void OnTrickBInput(TrickBInputEvent evt)
        {
            if (playerController != null && !playerModel.IsGrounded.Value)
            {
                // 直接切换到技巧状态，传递技巧名称
                playerController.stateMachine.SwitchState(StateLayer.Action, "TrickB");
                                 // Debug.Log("处理技巧B输入");
            }
        }

        
        private void OnJumpExecute(JumpExecuteEvent evt)
        {
            Debug.Log("PlayerSystem接收到JumpExecuteEvent - 执行跳跃");
            if (playerController != null)
            {
                // 系统层执行跳跃逻辑
                var rb = playerController.GetRigidbody();
                if (rb != null)
                {
                    // 获取当前水平速度
                    float currentHorizontalVelocity = rb.linearVelocity.x;
                    
                    // 执行跳跃
                    float jumpForce = playerModel.Config.Value.maxJumpForce;
                    rb.linearVelocity = new Vector2(currentHorizontalVelocity, jumpForce);
                    
                    Debug.Log($"系统执行跳跃 - 使用跳跃力: {jumpForce}, 水平速度: {currentHorizontalVelocity}");
                    
                    // 立即切换到Air状态
                    playerController.stateMachine.SwitchState(StateLayer.Movement, "Air");
                }
                else
                {
                    Debug.LogError("Rigidbody2D为空！");
                }
            }
            else
            {
                Debug.LogError("playerController为空！");
            }
        }

        private void OnGrindInput(GrindInputEvent evt)
        {
            Debug.Log($"OnGrindInput被调用:");
            Debug.Log($"  - playerController: {(playerController != null ? "存在" : "null")}");
            Debug.Log($"  - IsGrounded(): {(playerController != null ? playerModel.IsGrounded.Value.ToString() : "N/A")}");
            Debug.Log($"  - isNearTrack: {(playerController != null ? playerModel.IsNearTrack.Value.ToString() : "N/A")}");
            Debug.Log($"  - grindJumpTimer: {(playerController != null ? playerModel.GrindJumpTimer.Value.ToString() : "N/A")}");
            Debug.Log($"  - isNearWall: {(playerController != null ? playerModel.IsNearWall.Value.ToString() : "N/A")}");
            
            if (playerController != null)
            {
                // 如果在地面上且靠近滑轨，直接切换到滑轨状态
                if (playerModel.IsGrounded.Value && playerModel.IsNearTrack.Value && playerModel.GrindJumpTimer.Value <= 0f)
                {
                    Debug.Log("在地面上切换到Grind状态");
                    playerController.stateMachine.SwitchState(StateLayer.Action, "Grind");
                }
                // 如果在空中且靠近滑轨
                else if (!playerModel.IsGrounded.Value && playerModel.IsNearTrack.Value && playerModel.GrindJumpTimer.Value <= 0f)
                {
                    Debug.Log("在空中切换到Grind状态");
                    playerController.stateMachine.SwitchState(StateLayer.Action, "Grind");
                }
                // 如果在空中且靠近墙壁
                else if (!playerModel.IsGrounded.Value && playerModel.IsNearWall.Value)
                {
                    Debug.Log("切换到WallRide状态");
                    playerController.stateMachine.SwitchState(StateLayer.Action, "WallRide");
                }
                // 如果在空中但不在滑轨或墙壁附近
                else if (!playerModel.IsGrounded.Value)
                {
                    Debug.Log("切换到Grab状态");
                    playerController.stateMachine.SwitchState(StateLayer.Action, "Grab");
                }
                else
                {
                    Debug.Log("在地面上但不在滑轨附近，不切换状态");
                }
            }
            else
            {
                Debug.Log("OnGrindInput条件不满足，无法切换状态");
            }
        }
        
        private void OnPowerGrindInput(PowerGrindInputEvent evt)
        {
            if (playerController != null && playerModel.IsGrounded.Value)
            {
                playerController.stateMachine.SwitchState(StateLayer.Movement, "PowerGrind");
                // Debug.Log("切换到强力轨道状态");
            }
        }
        
        private void OnReverseInput(ReverseInputEvent evt)
        {
            if (playerController != null && playerModel.IsGrounded.Value)
            {
                playerController.stateMachine.SwitchState(StateLayer.Movement, "Reverse");
                // Debug.Log("切换到反向状态");
            }
        }

        private void OnStateChanged(StateChangedEvent evt)
        {
            // Debug.Log($"状态切换: {evt.FromState} -> {evt.ToState}");
        }

        // 使用 Raycast 检测 InteractiveLayer 对象
        private void DetectInteractiveObjectsWithRaycast()
        {
            if (playerController == null) return;

            Vector2 playerPosition = playerController.transform.position;
            float detectionRadius = 2f; // 检测半径
            
            // 方法1: 使用 Physics2D.OverlapCircle 检测圆形区域
            Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition, detectionRadius, LayerMask.GetMask("InteractiveLayer"));
            if(colliders.Length > 0)
            {
                this.GetModel<IPlayerModel>().isInPower.Value = true;
                if(this.GetModel<IPlayerModel>().isInPower.Value){
                    playerController.TrickABoostEffect.PlayFeedbacks();
                
                }
            }
        }
    }
}
