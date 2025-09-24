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
        private IPlayerModel playerModel;
        
        protected override void OnInit()
        {
            playerModel = this.GetModel<IPlayerModel>();
            playerController = Object.FindFirstObjectByType<InputController>();
            this.RegisterEvent<MoveInputEvent>(OnMoveInput);
        }
        
        private void OnMoveInput(MoveInputEvent evt)
        {
            if (playerController == null) return;
            
            string currentState = playerController.stateMachine.GetCurrentStateName();
            Debug.Log("currentState:" + currentState);
            
            switch (currentState)
            {
                case "Idle":
                    break;
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

            float currentSpeed = rb.linearVelocity.x;
            float targetSpeed = horizontalInput * playerModel.moveSpeed.Value;
            float newSpeed = currentSpeed;

            // ------------------------
            // 1. 有输入时
            // ------------------------
            if (Mathf.Abs(horizontalInput) > 0.01f)
            {
                // (1) 转向时，先减速 → 不能立刻掉头
                if (Mathf.Sign(currentSpeed) != Mathf.Sign(horizontalInput) && Mathf.Abs(currentSpeed) > 0.1f)
                {
                    newSpeed = Mathf.MoveTowards(currentSpeed, 0, playerModel.turnDecel.Value * Time.fixedDeltaTime);
                }
                else
                {
                    // (2) 同方向，加速逼近目标速度
                    newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, playerModel.groundAccel.Value * Time.fixedDeltaTime);
                }
            }
            // ------------------------
            // 2. 没有输入时 → 滑行一段再停下
            // ------------------------
            else
            {
                newSpeed = Mathf.MoveTowards(currentSpeed, 0, playerModel.groundDecel.Value * Time.fixedDeltaTime);
            }

            rb.linearVelocity = new Vector2(newSpeed, rb.linearVelocity.y);
        }
        
        public void ApplyAirMovement(float horizontalInput)
        {
            if (playerController == null) return;
            
            Rigidbody2D rb = playerController.GetRigidbody();
            if (rb == null) return;
            
            // 空中移动：施加力，并限制最大速度
            if (Mathf.Abs(horizontalInput) > 0.01f)
            {
                rb.AddForce(Vector2.right * horizontalInput * playerModel.airControlForce.Value, ForceMode2D.Force);
                
                // 控制最大水平速度
                float maxSpeed = playerModel.maxAirHorizontalSpeed.Value;
                rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed), rb.linearVelocity.y);
            }
            
            //Debug.Log($"空中移动: 输入={horizontalInput}, 当前速度={rb.linearVelocity.x}");
        }
        
        public void ApplyJumpMovement(float horizontalInput)
        {
            if (playerController == null) return;
            
            Rigidbody2D rb = playerController.GetRigidbody();
            if (rb == null) return;
            
            // 跳跃中的移动：使用力来控制，保持垂直速度
            if (Mathf.Abs(horizontalInput) > 0.01f)
            {
                rb.AddForce(Vector2.right * horizontalInput * playerModel.airControlForce.Value, ForceMode2D.Force);
                
                // 控制最大水平速度
                float maxSpeed = playerModel.maxAirHorizontalSpeed.Value;
                rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed), rb.linearVelocity.y);
            }
            
            Debug.Log($"跳跃移动: 输入={horizontalInput}, 当前速度={rb.linearVelocity.x}");
        }
    }
}
