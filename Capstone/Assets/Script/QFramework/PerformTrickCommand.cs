using QFramework;

namespace SkateGame
{
    public class PerformTrickCommand : ICommand, ICanSetArchitecture, IBelongToArchitecture
    {
        public string TrickName;
        private IArchitecture mArchitecture;
        
        public void Execute()
        {
            // 通过架构获取技巧系统并执行技巧
            mArchitecture.GetSystem<ITrickSystem>().PerformTrick(TrickName);
        }
        
        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
        
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }
    }
}
