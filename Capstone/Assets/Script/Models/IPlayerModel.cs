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
        BindableProperty<bool> CanDoubleJump { get; }
        // grind相关
        BindableProperty<float> Speed { get; }
        BindableProperty<Vector2> Direction { get; }
        BindableProperty<bool> IsJumping { get; }
        BindableProperty<float> NormalG { get; }

        // jump相关
        BindableProperty<float> ChargeTime { get; }
        BindableProperty<bool> IsCharging { get; }
        BindableProperty<bool> HasJumped { get; }
        BindableProperty<float> InitialHorizontalVelocity { get; }

        // move相关
        BindableProperty<float> CurrentVelocityX { get; }
        BindableProperty<float> Acceleration { get; }
        BindableProperty<float> Deceleration { get; }
        BindableProperty<float> MaxSpeed { get; }
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
        public BindableProperty<float> JumpForce { get; } = new BindableProperty<float>(14f);
        public BindableProperty<bool> CanDoubleJump { get; } = new BindableProperty<bool>(false);

        // grind相关
        public BindableProperty<float> Speed { get; } = new BindableProperty<float>(0f);
        public BindableProperty<Vector2> Direction { get; } = new BindableProperty<Vector2>(Vector2.zero);
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
        public BindableProperty<float> Deceleration { get; } = new BindableProperty<float>(20f);
        public BindableProperty<float> MaxSpeed { get; } = new BindableProperty<float>(5f);
         
        
        protected override void OnInit()
        {
            // 初始化逻辑
        }
    }
}
