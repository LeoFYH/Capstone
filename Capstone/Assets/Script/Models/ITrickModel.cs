using QFramework;
using System.Collections.Generic;

namespace SkateGame
{
    public interface ITrickModel : IModel
    {
        BindableProperty<List<TrickInfo>> CurrentTricks { get; }
        BindableProperty<string> CurrentTrickName { get; }
        BindableProperty<bool> IsPerformingTrick { get; }
        BindableProperty<float> TrickTimer { get; }
    }

    public class TrickModel : AbstractModel, ITrickModel
    {
        public BindableProperty<List<TrickInfo>> CurrentTricks { get; } = new BindableProperty<List<TrickInfo>>(new List<TrickInfo>());
        public BindableProperty<string> CurrentTrickName { get; } = new BindableProperty<string>("");
        public BindableProperty<bool> IsPerformingTrick { get; } = new BindableProperty<bool>(false);
        public BindableProperty<float> TrickTimer { get; } = new BindableProperty<float>(0f);
        
        protected override void OnInit()
        {
            // 初始化逻辑
        }
    }
}
