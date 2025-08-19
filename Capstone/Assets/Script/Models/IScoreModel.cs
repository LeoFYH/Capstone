using QFramework;

namespace SkateGame
{
    public interface IScoreModel : IModel
    {
        BindableProperty<int> TotalScore { get; }
        BindableProperty<int> ComboMultiplier { get; }
    }

    public class ScoreModel : AbstractModel, IScoreModel
    {
        public BindableProperty<int> TotalScore { get; } = new BindableProperty<int>(0);
        public BindableProperty<int> ComboMultiplier { get; } = new BindableProperty<int>(1);
        
        protected override void OnInit()
        {
            // 初始化逻辑
        }
    }
}
