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
            this.RegisterModel<IGameModel>(new GameModel());
            this.RegisterModel<ITrickListModel>(new TrickListModel());
            this.RegisterModel<ILevelModel>(new LevelModel());

            this.RegisterModel<IEnemyModel>(new EnemyModel());

            // 注册业务系统（PlayerAssetSystem 最先初始化，确保配置优先加载）
            this.RegisterSystem<IPlayerAssetSystem>(new PlayerAssetSystem());
            this.RegisterSystem<IEnemyAssetSystem>(new EnemyAssetSystem());
            this.RegisterSystem<IPlayerSystem>(new PlayerSystem());
            this.RegisterSystem<ITrickSystem>(new TrickSystem());
            this.RegisterSystem<ILevelSystem>(new LevelSystem());
        }
    }
}
