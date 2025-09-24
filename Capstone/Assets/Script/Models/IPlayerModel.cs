using QFramework;
using UnityEngine;

namespace SkateGame
{
    public interface IPlayerModel : IModel
    {
        BindableProperty<string> CurrentState { get; }
        BindableProperty<bool> IsGrounded { get; }
        BindableProperty<bool> IsInAir { get; }
        BindableProperty<float> AirTime { get; }
        BindableProperty<int> AirCombo { get; }
        BindableProperty<bool> IsNearTrack { get; }
        BindableProperty<bool> IsNearWall { get; }
        BindableProperty<float> MoveSpeed { get; }
        BindableProperty<float> JumpForce { get; }
        BindableProperty<float> StateTimer { get; }

        /// <summary>
        /// air相关
        /// </summary>
        BindableProperty<float> AirControlForce { get; }
        BindableProperty<float> MaxAirHorizontalSpeed { get; }
        BindableProperty<bool> CanDoubleJump { get; }

        /// <summary>
        /// grind相关
        /// </summary>
        BindableProperty<float> Speed { get; }
        BindableProperty<Vector2> GrindDirection { get; }
        BindableProperty<bool> IsJumping { get; }
        BindableProperty<float> NormalG { get; }

        /// <summary>
        /// jump相关
        /// </summary>
        BindableProperty<float> ChargeTime { get; }
        BindableProperty<bool> IsCharging { get; }
        BindableProperty<bool> HasJumped { get; }
        BindableProperty<float> InitialHorizontalVelocity { get; }

        /// <summary>
        /// move相关
        /// </summary>
        BindableProperty<float> CurrentVelocityX { get; }
        BindableProperty<float> Acceleration { get; }
        BindableProperty<float> MoveDeceleration { get; }
        BindableProperty<float> MaxSpeed { get; }

        /// <summary>
        /// power grind相关
        /// </summary>
        BindableProperty<float> PowerGrindDeceleration { get; }
        BindableProperty<float> PowerGrindDirection { get; }

        /// <summary>
        /// Trick相关
        /// </summary>
        BindableProperty<float> TrickADuration { get; }
        BindableProperty<int> TrickAScore { get; }
        BindableProperty<float> TrickBDuration { get; }
        BindableProperty<int> TrickBScore { get; }
        BindableProperty<bool> isInPower { get; }

        ///———————从inputcontroller迁移———————
        /// <summary>
        /// 跳跃设置
        /// </summary>
        BindableProperty<float> maxJumpForce { get; }
        BindableProperty<float> minJumpForce { get; }
        BindableProperty<float> doubleJumpForce { get; }
        BindableProperty<float> maxChargeTime { get; }
        BindableProperty<bool> AllowDoubleJump { get; }
        [Header("移动设置")]
        BindableProperty<float> moveSpeed { get; }
        BindableProperty<float> airControlForce { get; }
        BindableProperty<float> maxAirHorizontalSpeed { get; }
        BindableProperty<float> groundAccel { get; }
        BindableProperty<float> groundDecel { get; }
        BindableProperty<float> turnDecel { get; }
        
        // 非输入的运行时/调参（来自 InputController，但不包含 isEHeld/isWHeld 等原始输入）
        BindableProperty<Track> CurrentTrack { get; }
        BindableProperty<float> GrindJumpTimer { get; } // 用来防止跳跃后被吸附到原先滑轨
        BindableProperty<bool> WasGrounded { get; }
        BindableProperty<bool> IsCheckingReverseWindow { get; }
        BindableProperty<float> ReverseTimer { get; }
        BindableProperty<bool> IsAiming { get; }
        BindableProperty<float> MaxAimTime { get; }
        BindableProperty<float> AimTimer { get; }
        BindableProperty<int> CurrentBulletIndex { get; }
        BindableProperty<bool> HasPerformedTrickInAir { get; }
    }

    public class PlayerModel : AbstractModel, IPlayerModel
    {
        public BindableProperty<string> CurrentState { get; } = new BindableProperty<string>("Idle");
        public BindableProperty<bool> IsGrounded { get; } = new BindableProperty<bool>(true);
        public BindableProperty<bool> IsInAir { get; } = new BindableProperty<bool>(false);
        public BindableProperty<float> AirTime { get; } = new BindableProperty<float>(0f);
        public BindableProperty<int> AirCombo { get; } = new BindableProperty<int>(0);
        public BindableProperty<bool> IsNearTrack { get; } = new BindableProperty<bool>(false);
        public BindableProperty<bool> IsNearWall { get; } = new BindableProperty<bool>(false);
        public BindableProperty<float> MoveSpeed { get; } = new BindableProperty<float>(5f);
        public BindableProperty<float> JumpForce { get; } = new BindableProperty<float>(0f);
        public BindableProperty<float> StateTimer { get; } = new BindableProperty<float>(0f);
        
        // air相关
        public BindableProperty<float> AirControlForce { get; } = new BindableProperty<float>(10f);
        public BindableProperty<float> MaxAirHorizontalSpeed { get; } = new BindableProperty<float>(8f);
        public BindableProperty<bool> CanDoubleJump { get; } = new BindableProperty<bool>(true);

        // grind相关
        public BindableProperty<float> Speed { get; } = new BindableProperty<float>(0f);
        public BindableProperty<Vector2> GrindDirection { get; } = new BindableProperty<Vector2>(Vector2.zero);
        public BindableProperty<bool> IsJumping { get; } = new BindableProperty<bool>(false);
        public BindableProperty<float> NormalG { get; } = new BindableProperty<float>(1f);
        
        // jump相关
        public BindableProperty<float> ChargeTime { get; } = new BindableProperty<float>(0f);
        public BindableProperty<bool> IsCharging { get; } = new BindableProperty<bool>(false);
        public BindableProperty<bool> HasJumped { get; } = new BindableProperty<bool>(false);
        public BindableProperty<float> InitialHorizontalVelocity { get; } = new BindableProperty<float>(0f);
        
        // move相关
        public BindableProperty<float> CurrentVelocityX { get; } = new BindableProperty<float>(0f);
        public BindableProperty<float> Acceleration { get; } = new BindableProperty<float>(15f);
        public BindableProperty<float> MoveDeceleration { get; } = new BindableProperty<float>(20f);
        public BindableProperty<float> MaxSpeed { get; } = new BindableProperty<float>(5f);
         
        // power grind相关
        public BindableProperty<float> PowerGrindDeceleration { get; } = new BindableProperty<float>(1f);
        public BindableProperty<float> PowerGrindDirection { get; } = new BindableProperty<float>(0f);

        // Trick相关
        public BindableProperty<float> TrickADuration { get; } = new BindableProperty<float>(1.5f);
        public BindableProperty<int> TrickAScore { get; } = new BindableProperty<int>(20);
        public BindableProperty<float> TrickBDuration { get; } = new BindableProperty<float>(1.5f);
        public BindableProperty<int> TrickBScore { get; } = new BindableProperty<int>(20);
        public BindableProperty<bool> isInPower { get; } = new BindableProperty<bool>(false);

        // 跳跃设置
        public BindableProperty<float> maxJumpForce { get; } = new BindableProperty<float>(6f);
        public BindableProperty<float> minJumpForce { get; } = new BindableProperty<float>(0f);
        public BindableProperty<float> doubleJumpForce { get; } = new BindableProperty<float>(8f);
        public BindableProperty<float> maxChargeTime { get; } = new BindableProperty<float>(2f);
        public BindableProperty<bool> AllowDoubleJump { get; } = new BindableProperty<bool>(true);

        // 移动设置
        public BindableProperty<float> moveSpeed { get; } = new BindableProperty<float>(5f);
        public BindableProperty<float> airControlForce { get; } = new BindableProperty<float>(5f);
        public BindableProperty<float> maxAirHorizontalSpeed { get; } = new BindableProperty<float>(10f);
        public BindableProperty<float> groundAccel { get; } = new BindableProperty<float>(20f);
        public BindableProperty<float> groundDecel { get; } = new BindableProperty<float>(10f);
        public BindableProperty<float> turnDecel { get; } = new BindableProperty<float>(40f);

        // 非输入的运行时/调参（来自 InputController，但不包含 isEHeld/isWHeld 等原始输入）
        public BindableProperty<Track> CurrentTrack { get; } = new BindableProperty<Track>(null);
        public BindableProperty<float> GrindJumpTimer { get; } = new BindableProperty<float>(0f);
        public BindableProperty<bool> WasGrounded { get; } = new BindableProperty<bool>(true);
        public BindableProperty<bool> IsCheckingReverseWindow { get; } = new BindableProperty<bool>(false);
        public BindableProperty<float> ReverseTimer { get; } = new BindableProperty<float>(0f);
        public BindableProperty<bool> IsAiming { get; } = new BindableProperty<bool>(false);
        public BindableProperty<float> MaxAimTime { get; } = new BindableProperty<float>(3f);
        public BindableProperty<float> AimTimer { get; } = new BindableProperty<float>(0f);
        public BindableProperty<int> CurrentBulletIndex { get; } = new BindableProperty<int>(0);
        public BindableProperty<bool> HasPerformedTrickInAir { get; } = new BindableProperty<bool>(false);
        protected override void OnInit()
        {
            // 初始化逻辑
        }
    }
}
