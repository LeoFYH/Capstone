using QFramework;
using UnityEngine;

namespace SkateGame
{
    public interface IMovementSystem : ISystem
    {
        void ApplyGroundMovement(float horizontalInput);
        void ApplyAirMovement(float horizontalInput);
        void ApplyJumpMovement(float horizontalInput);
    }

    public class MovementSystem : AbstractSystem, IMovementSystem
    {
        private InputController playerController;
        
        protected override void OnInit()
        {
            playerController = Object.FindFirstObjectByType<InputController>();
            this.RegisterEvent<MoveInputEvent>(OnMoveInput);
        }
        
        private void OnMoveInput(MoveInputEvent evt)
        {
            if (playerController == null) return;
            
            string currentState = playerController.stateMachine.GetCurrentStateName();
            
            switch (currentState)
            {
                case "Idle":
                case "Move":
                    ApplyGroundMovement(evt.HorizontalInput);
                    break;
                case "Air":
                    ApplyAirMovement(evt.HorizontalInput);
                    break;
                case "Jump":
                    ApplyJumpMovement(evt.HorizontalInput);
                    break;
                // 其他状态可以根据需要添加
            }
        }
        
        public void ApplyGroundMovement(float horizontalInput)
        {
            if (playerController == null || !playerController.IsGrounded()) return;
            
            Rigidbody2D rb = playerController.GetRigidbody();
            if (rb == null) return;
            
            // 地面移动：直接设置速度
            float targetVelocityX = horizontalInput * playerController.moveSpeed;
            rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);
            
            //Debug.Log($"地面移动: 输入={horizontalInput}, 目标速度={targetVelocityX}");
        }
        
        public void ApplyAirMovement(float horizontalInput)
        {
            if (playerController == null) return;
            
            Rigidbody2D rb = playerController.GetRigidbody();
            if (rb == null) return;
            
            // 空中移动：施加力，并限制最大速度
            if (Mathf.Abs(horizontalInput) > 0.01f)
            {
                rb.AddForce(Vector2.right * horizontalInput * playerController.airControlForce, ForceMode2D.Force);
                
                // 控制最大水平速度
                float maxSpeed = playerController.maxAirHorizontalSpeed;
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
            }
            
            // Debug.Log($"空中移动: 输入={horizontalInput}, 当前速度={rb.velocity.x}");
        }
        
        public void ApplyJumpMovement(float horizontalInput)
        {
            if (playerController == null) return;
            
            Rigidbody2D rb = playerController.GetRigidbody();
            if (rb == null) return;
            
            // 跳跃中的移动：使用力来控制，保持垂直速度
            if (Mathf.Abs(horizontalInput) > 0.01f)
            {
                rb.AddForce(Vector2.right * horizontalInput * playerController.airControlForce, ForceMode2D.Force);
                
                // 控制最大水平速度
                float maxSpeed = playerController.maxAirHorizontalSpeed;
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
            }
            
            // Debug.Log($"跳跃移动: 输入={horizontalInput}, 当前速度={rb.velocity.x}");
        }
    }
}
