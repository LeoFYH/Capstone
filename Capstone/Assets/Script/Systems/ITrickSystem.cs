using QFramework;
using UnityEngine;
using System.Collections.Generic;

namespace SkateGame
{
    public interface ITrickSystem : ISystem
    {
        void PerformTrick(string trickName);
        void AddTrickScore(TrickBase trick);
        void ResetTrickScore();
        void UpdateTrickTimer(float deltaTime);
    }

    public class TrickSystem : AbstractSystem, ITrickSystem
    {
        protected override void OnInit()
        {
            // 监听玩家落地事件
            this.RegisterEvent<PlayerLandedEvent>(OnPlayerLanded);
        }

        public void PerformTrick(string trickName)
        {
            var trickModel = this.GetModel<ITrickModel>();
            trickModel.CurrentTrickName.Value = trickName;
            
            // 获取玩家控制器
            InputController playerController = Object.FindFirstObjectByType<InputController>();
            
            // 创建技巧实例
            TrickBase trick = CreateTrick(trickName);
            if (trick != null && playerController != null)
            {
                trick.PerformTrick(playerController);
                trickModel.IsPerformingTrick.Value = true;
                trickModel.TrickTimer.Value = trick.duration;
                
                // 添加分数
                AddTrickScore(trick);
                
                // 发送技巧执行事件
                this.SendEvent<TrickPerformedEvent>(new TrickPerformedEvent { TrickName = trickName });
            }
        }

        public void AddTrickScore(TrickBase trick)
        {
            var trickModel = this.GetModel<ITrickModel>();
            var scoreModel = this.GetModel<IScoreModel>();
            
            TrickInfo trickInfo = new TrickInfo(trick);
            trickModel.CurrentTricks.Value.Add(trickInfo);
            scoreModel.TotalScore.Value += trick.scoreValue;
            
            // Debug.Log($"添加技巧: {trick.trickName}, 分数: {trick.scoreValue}");
        }

        public void ResetTrickScore()
        {
            var trickModel = this.GetModel<ITrickModel>();
            var scoreModel = this.GetModel<IScoreModel>();
            
            trickModel.CurrentTricks.Value.Clear();
            scoreModel.TotalScore.Value = 0;
        }

        public void UpdateTrickTimer(float deltaTime)
        {
            var trickModel = this.GetModel<ITrickModel>();
            
            if (trickModel.TrickTimer.Value > 0)
            {
                trickModel.TrickTimer.Value -= deltaTime;
                
                if (trickModel.TrickTimer.Value <= 0)
                {
                    trickModel.IsPerformingTrick.Value = false;
                    this.SendEvent<TrickCompletedEvent>();
                }
            }
        }

        private TrickBase CreateTrick(string trickName)
        {
            switch (trickName)
            {
                case "TrickA":
                    return new TrickA();
                case "TrickB":
                    return new TrickB();
                case "TrickC":
                    return new TrickC();
                default:
                    Debug.LogWarning($"未知的技巧: {trickName}");
                    return null;
            }
        }

        private void OnPlayerLanded(PlayerLandedEvent evt)
        {
            ResetTrickScore();
            // Debug.Log("玩家落地，清空技巧分数");
        }
    }
}
