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
            // 监听技巧执行事件
            this.RegisterEvent<TrickPerformedEvent>(OnTrickPerformed);
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
                
                // 不再发送事件，直接更新模型
                // UIController会监听模型变化自动更新UI
            }
        }

        public void AddTrickScore(TrickBase trick)
        {
            var trickModel = this.GetModel<ITrickModel>();
            var scoreModel = this.GetModel<IScoreModel>();
            
            TrickInfo trickInfo = new TrickInfo(trick);
            trickModel.CurrentTricks.Value.Add(trickInfo);
            scoreModel.TotalScore.Value += trick.scoreValue;
            
            Debug.Log($"添加技巧: {trick.trickName}, 分数: {trick.scoreValue}");
        }

        public void ResetTrickScore()
        {
            var trickModel = this.GetModel<ITrickModel>();
            var scoreModel = this.GetModel<IScoreModel>();
            
            trickModel.CurrentTricks.Value.Clear();
            scoreModel.TotalScore.Value = 0;
            trickModel.CurrentTrickName.Value = "";
            trickModel.IsPerformingTrick.Value = false;
            trickModel.TrickTimer.Value = 0f;
            
            Debug.Log("TrickSystem: 重置技巧分数");
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
                case "doublejump":
                    // 为测试创建一个简单的技巧实例
                    var doubleJumpTrick = new TrickA(); // 复用TrickA的结构
                    doubleJumpTrick.trickName = "doublejump";
                    doubleJumpTrick.scoreValue = 10;
                    doubleJumpTrick.duration = 0.5f;
                    return doubleJumpTrick;
                default:
                    Debug.LogWarning($"未知的技巧: {trickName}");
                    return null;
            }
        }

        private void OnPlayerLanded(PlayerLandedEvent evt)
        {
            ResetTrickScore();
            Debug.Log("玩家落地，清空技巧分数");
        }
        
        private void OnTrickPerformed(TrickPerformedEvent evt)
        {
            Debug.Log($"TrickSystem: 处理技巧执行事件 - {evt.TrickName}");
            
            // 创建技巧实例
            TrickBase trick = CreateTrick(evt.TrickName);
            if (trick != null)
            {
                Debug.Log($"TrickSystem: 创建技巧实例成功: {evt.TrickName}");
                
                // 更新模型数据
                var trickModel = this.GetModel<ITrickModel>();
                var scoreModel = this.GetModel<IScoreModel>();
                
                if (trickModel == null)
                {
                    Debug.LogError("TrickSystem: trickModel为空！");
                    return;
                }
                
                if (scoreModel == null)
                {
                    Debug.LogError("TrickSystem: scoreModel为空！");
                    return;
                }
                
                // 添加技巧信息
                TrickInfo trickInfo = new TrickInfo(trick);
                trickModel.CurrentTricks.Value.Add(trickInfo);
                trickModel.CurrentTrickName.Value = evt.TrickName;
                trickModel.IsPerformingTrick.Value = true;
                trickModel.TrickTimer.Value = trick.duration;
                
                // 更新分数
                scoreModel.TotalScore.Value += trick.scoreValue;
                
                Debug.Log($"TrickSystem: 添加技巧 {evt.TrickName}, 分数: {trick.scoreValue}, 总分: {scoreModel.TotalScore.Value}");
                Debug.Log($"TrickSystem: 技巧列表数量: {trickModel.CurrentTricks.Value.Count}");
            }
            else
            {
                Debug.LogError($"TrickSystem: 创建技巧实例失败: {evt.TrickName}");
            }
        }
    }
}
