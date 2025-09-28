using QFramework;
using UnityEngine;

namespace SkateGame
{
    public interface IPlayerAssetSystem : ISystem
    {
        void LoadConfigToModel();
        void SetPlayerConfig(PlayerConfig config);
        PlayerConfig GetPlayerConfig();
    }

    public class PlayerAssetSystem : AbstractSystem, IPlayerAssetSystem
    {
        private PlayerConfig playerConfig;
        private IPlayerModel playerModel;
        
        protected override void OnInit()
        {
            // 获取玩家模型
            playerModel = this.GetModel<IPlayerModel>();
            
            // 尝试自动查找配置资源
            LoadConfigFromResources();
        }

        public void LoadConfigToModel()
        {

            // also store the reference to config on the model
            playerModel.Config.Value = playerConfig;
            Debug.Log("PlayerAssetSystem: 配置参数已成功加载到模型");
        }

        public void SetPlayerConfig(PlayerConfig config)
        {   
            // 如果模型已初始化，立即加载配置
            if (playerModel != null )
            {
                LoadConfigToModel();
            }
        }

        public PlayerConfig GetPlayerConfig()
        {
            return playerConfig;
        }

        private void LoadConfigFromResources()
        {
            // 尝试从 Assets 根目录查找
            PlayerConfig[] allConfigs = Resources.FindObjectsOfTypeAll<PlayerConfig>();
            if (allConfigs.Length > 0)
            {
                SetPlayerConfig(allConfigs[0]);
                return;
            }
        }
    }
}
