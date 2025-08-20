using QFramework;
using UnityEngine;

namespace SkateGame
{
    public interface IPlayerLandingSystem : ISystem
    {
        void HandlePlayerLanded();
        void ResetTrickScore();
        void ShowLandingResults();
    }

    public class PlayerLandingSystem : AbstractSystem, IPlayerLandingSystem
    {
        protected override void OnInit()
        {
            // 监听玩家落地事件
            this.RegisterEvent<PlayerLandedEvent>(OnPlayerLanded);
            
            Debug.Log("PlayerLandingSystem初始化完成");
        }

        public void HandlePlayerLanded()
        {
            Debug.Log("处理玩家落地");
            
            // 显示落地结果
            ShowLandingResults();
            
            // 重置技巧分数
            ResetTrickScore();
        }

        public void ResetTrickScore()
        {
            // 通过TrickScore重置分数
            TrickScore.Instance.ResetTrickScore();
            
            // 发送UI清理事件
            this.SendEvent<UIClearEvent>(new UIClearEvent { ClearType = UIClearType.All });
        }

        public void ShowLandingResults()
        {
            var trickScore = TrickScore.Instance;
            
            if (trickScore.trickInfos.Count > 0)
            {
                Debug.Log("=== 玩家落地，技巧列表 ===");
                trickScore.PrintAllTricks();
                Debug.Log("5秒后清零技巧列表...");
                
                // 可以在这里添加更多落地效果，比如：
                // - 播放落地音效
                // - 显示落地特效
                // - 更新UI显示
                // - 保存最高分记录
            }
        }

        private void OnPlayerLanded(PlayerLandedEvent evt)
        {
            HandlePlayerLanded();
        }
    }
}
