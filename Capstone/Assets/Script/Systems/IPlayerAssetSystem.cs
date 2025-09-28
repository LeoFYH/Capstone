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
            if (playerConfig == null)
            {
                Debug.LogWarning("PlayerAssetSystem: PlayerConfig 为空，无法加载配置到模型");
                return;
            }

            if (playerModel == null)
            {
                Debug.LogError("PlayerAssetSystem: PlayerModel 为空，无法加载配置");
                return;
            }

            Debug.Log("PlayerAssetSystem: 开始从配置加载参数到模型");

            // 跳跃设置
            playerModel.maxJumpForce.Value = playerConfig.maxJumpForce;
            playerModel.minJumpForce.Value = playerConfig.minJumpForce;
            playerModel.doubleJumpForce.Value = playerConfig.doubleJumpForce;
            playerModel.maxChargeTime.Value = playerConfig.maxChargeTime;
            playerModel.AllowDoubleJump.Value = playerConfig.allowDoubleJump;

            // 移动设置
            playerModel.moveSpeed.Value = playerConfig.moveSpeed;
            playerModel.airControlForce.Value = playerConfig.airControlForce;
            playerModel.maxAirHorizontalSpeed.Value = playerConfig.maxAirHorizontalSpeed;
            playerModel.groundAccel.Value = playerConfig.groundAccel;
            playerModel.groundDecel.Value = playerConfig.groundDecel;
            playerModel.turnDecel.Value = playerConfig.turnDecel;

            // Air相关
            playerModel.AirControlForce.Value = playerConfig.airControlForceConfig;
            playerModel.MaxAirHorizontalSpeed.Value = playerConfig.maxAirHorizontalSpeedConfig;

            // Grind相关
            playerModel.NormalG.Value = playerConfig.normalG;

            // Move相关
            playerModel.Acceleration.Value = playerConfig.acceleration;
            playerModel.MoveDeceleration.Value = playerConfig.moveDeceleration;
            playerModel.MaxSpeed.Value = playerConfig.maxSpeed;

            // Power Grind相关
            playerModel.PowerGrindDeceleration.Value = playerConfig.powerGrindDeceleration;

            // Trick相关
            playerModel.TrickADuration.Value = playerConfig.trickADuration;
            playerModel.TrickAScore.Value = playerConfig.trickAScore;
            playerModel.TrickBDuration.Value = playerConfig.trickBDuration;
            playerModel.TrickBScore.Value = playerConfig.trickBScore;

            // 瞄准设置
            playerModel.MaxAimTime.Value = playerConfig.maxAimTime;

            Debug.Log("PlayerAssetSystem: 配置参数已成功加载到模型");
        }

        public void SetPlayerConfig(PlayerConfig config)
        {
            playerConfig = config;
            Debug.Log($"PlayerAssetSystem: 设置 PlayerConfig - {(config != null ? "成功" : "为空")}");
            
            // 如果模型已初始化，立即加载配置
            if (playerModel != null && config != null)
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
           
         
            
            // 方式2：尝试从 Assets 根目录查找
            PlayerConfig[] allConfigs = Resources.FindObjectsOfTypeAll<PlayerConfig>();
            if (allConfigs.Length > 0)
            {
                SetPlayerConfig(allConfigs[0]);
                Debug.Log($"PlayerAssetSystem: 从 Assets 找到并加载了 PlayerConfig: {allConfigs[0].name}");
                return;
            }
            
            Debug.LogWarning("PlayerAssetSystem: 未找到任何 PlayerConfig，请手动设置配置");
        }
    }
}
