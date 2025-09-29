using QFramework;
using UnityEngine;

namespace SkateGame
{
    public interface IPlayerSystem : ISystem
    {
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
            this.RegisterEvent<MoveInputEvent>(OnMoveInput);
            this.RegisterEvent<JumpExecuteEvent>(OnJumpInput);
        }

        // 输入事件处理
        
        #region Event
        private void OnMoveInput(MoveInputEvent evt)
        {
            ApplyHorizontalMovement(evt.HorizontalInput, playerModel.IsGrounded.Value);
        }
         private void OnJumpInput(JumpExecuteEvent evt)
        {
            ApplyJumpMovement();
        }
        #endregion

        #region Method
        public void ApplyHorizontalMovement(float horizontalInput, bool isGrounded)
        {
            Rigidbody2D rb = playerController.GetRigidbody();

            float currentSpeed = rb.linearVelocity.x;
            float targetSpeed = horizontalInput * (isGrounded ? playerModel.Config.Value.maxMoveSpeed : playerModel.Config.Value.maxAirHorizontalSpeed);
            float newSpeed = currentSpeed;

            // ------------------------
            // 1. 有输入时
            // ------------------------
            if (Mathf.Abs(horizontalInput) > 0.01f)
            {
                // (1) 转向时，先减速 → 不能立刻掉头
                if (Mathf.Sign(currentSpeed) != Mathf.Sign(horizontalInput) && Mathf.Abs(currentSpeed) > 0.1f)
                {
                    newSpeed = Mathf.MoveTowards(currentSpeed, 0, playerModel.Config.Value.turnDecel * Time.fixedDeltaTime);
                }
                else
                {
                    // (2) 同方向，加速逼近目标速度
                    newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, 
                    (isGrounded ? playerModel.Config.Value.groundAccel : playerModel.Config.Value.airAccel) * Time.fixedDeltaTime);
                }
            }
            // ------------------------
            // 2. 没有输入时 → 滑行
            // --------------------
            else
            {
                float decel = isGrounded ? playerModel.Config.Value.groundDecel : playerModel.Config.Value.airDecel;
                newSpeed = Mathf.MoveTowards(currentSpeed, 0, decel * Time.fixedDeltaTime);
            }

            rb.linearVelocity = new Vector2(newSpeed, rb.linearVelocity.y);
        }

        public void ApplyJumpMovement()
        {
            var rb = playerController.GetRigidbody();   
            float jumpForce = playerModel.Config.Value.maxJumpForce;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        #endregion
        
    }
}
