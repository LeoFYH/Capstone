using QFramework;

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
        BindableProperty<bool> canDoubleJump { get; }
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
        public BindableProperty<bool> canDoubleJump { get; } = new BindableProperty<bool>(false);
        
        protected override void OnInit()
        {
            // 初始化逻辑
        }
    }
}
