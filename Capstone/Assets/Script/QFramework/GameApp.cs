using QFramework;

namespace SkateGame
{
    public class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            // 注册数据模型
            this.RegisterModel<IPlayerModel>(new PlayerModel());
            this.RegisterModel<IInputModel>(new InputModel());
            this.RegisterModel<ITrickModel>(new TrickModel());
            this.RegisterModel<IScoreModel>(new ScoreModel());
            this.RegisterModel<IGameModel>(new GameModel());
            
            // 注册业务系统
            this.RegisterSystem<ITrickSystem>(new TrickSystem());
            this.RegisterSystem<IPlayerSystem>(new PlayerSystem());
            this.RegisterSystem<IScoreSystem>(new ScoreSystem());
            this.RegisterSystem<IAudioSystem>(new AudioSystem());
            this.RegisterSystem<IUISystem>(new UISystem());
            this.RegisterSystem<IMovementSystem>(new MovementSystem());
        }
    }
}
