using QFramework;
using UnityEngine;
using System.Collections.Generic;

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
        private IPlayerModel playerModel;
        
        protected override void OnInit()
        {
            // 获取玩家控制器
            playerController = Object.FindFirstObjectByType<InputController>();
            Debug.Log($"PlayerSystem初始化: playerController = {(playerController != null ? "找到" : "未找到")}");
            
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
            
            if (playerController != null && !playerController.IsGrounded())
            {
                Debug.Log("PlayerSystem: 在空中执行TrickA");
                playerController.stateMachine.SwitchState("Trick", "TrickA");
                
            }
            else
            {
                Debug.Log($"PlayerSystem: 无法执行技巧 - 在地面上或playerController为空");
            }
        }

        private void OnTrickBInput(TrickBInputEvent evt)
        {
            if (playerController != null && !playerController.IsGrounded())
            {
                // 直接切换到技巧状态，传递技巧名称
                playerController.stateMachine.SwitchState("Trick", "TrickB");
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
                    float jumpForce = playerModel.maxJumpForce.Value;
                    rb.linearVelocity = new Vector2(currentHorizontalVelocity, jumpForce);
                    
                    Debug.Log($"系统执行跳跃 - 使用跳跃力: {jumpForce}, 水平速度: {currentHorizontalVelocity}");
                    
                    // 立即切换到Air状态
                    playerController.stateMachine.SwitchState("Air");
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
            Debug.Log($"  - IsGrounded(): {(playerController != null ? playerController.IsGrounded().ToString() : "N/A")}");
            Debug.Log($"  - isNearTrack: {(playerController != null ? playerController.isNearTrack.ToString() : "N/A")}");
            Debug.Log($"  - grindJumpTimer: {(playerController != null ? playerController.grindJumpTimer.ToString() : "N/A")}");
            Debug.Log($"  - isNearWall: {(playerController != null ? playerController.isNearWall.ToString() : "N/A")}");
            
            if (playerController != null)
            {
                // 如果在地面上且靠近滑轨，直接切换到滑轨状态
                if (playerController.IsGrounded() && playerController.isNearTrack && playerController.grindJumpTimer <= 0f)
                {
                    Debug.Log("在地面上切换到Grind状态");
                    playerController.stateMachine.SwitchState("Grind");
                }
                // 如果在空中且靠近滑轨
                else if (!playerController.IsGrounded() && playerController.isNearTrack && playerController.grindJumpTimer <= 0f)
                {
                    Debug.Log("在空中切换到Grind状态");
                    playerController.stateMachine.SwitchState("Grind");
                }
                // 如果在空中且靠近墙壁
                else if (!playerController.IsGrounded() && playerController.isNearWall)
                {
                    Debug.Log("切换到WallRide状态");
                    playerController.stateMachine.SwitchState("WallRide");
                }
                // 如果在空中但不在滑轨或墙壁附近
                else if (!playerController.IsGrounded())
                {
                    Debug.Log("切换到Grab状态");
                    playerController.stateMachine.SwitchState("Grab");
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
            if (playerController != null && playerController.IsGrounded())
            {
                playerController.stateMachine.SwitchState("PowerGrind");
                // Debug.Log("切换到强力轨道状态");
            }
        }
        
        private void OnReverseInput(ReverseInputEvent evt)
        {
            if (playerController != null && playerController.IsGrounded())
            {
                playerController.stateMachine.SwitchState("Reverse");
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
            
            if (colliders.Length > 0)
            {
                Debug.Log($"Raycast检测到 {colliders.Length} 个 InteractiveLayer 对象:");
                foreach (var collider in colliders)
                {
                    Debug.Log($"  - 对象: {collider.name}");
                    Debug.Log($"  - 距离: {Vector2.Distance(playerPosition, collider.transform.position):F2}");
                    Debug.Log($"  - 是Trigger: {collider.isTrigger}");
                }
            }
            else
            {
                Debug.Log("Raycast没有检测到任何 InteractiveLayer 对象");
            }
            
           
        }
        
        // 使用多个方向的射线检测
       
    }
}
