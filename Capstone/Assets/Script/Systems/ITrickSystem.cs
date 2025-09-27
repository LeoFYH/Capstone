using QFramework;
using UnityEngine;
using System.Collections.Generic;

namespace SkateGame
{
    public interface ITrickSystem : ISystem
    {
        void PerformTrick(string trickName);
        void AddTrickScore(TrickState trick);
        void ResetTrickScore();
        void UpdateTrickTimer(float deltaTime);
    }

    public class TrickSystem : AbstractSystem, ITrickSystem
    {
        private InputController playerController;
        protected override void OnInit()
        {
            playerController = Object.FindFirstObjectByType<InputController>();
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
            TrickState trick = playerController.stateMachine.TryGetState(trickName, StateLayer.Action) as TrickState;
            if (trick != null && playerController != null)
            {
                trickModel.IsPerformingTrick.Value = true;
                trickModel.TrickTimer.Value = trick.StateTotalDuration;
                
                // 添加分数
                AddTrickScore(trick);
                
                // 不再发送事件，直接更新模型
                // UIController会监听模型变化自动更新UI
            }
        }

        public void AddTrickScore(TrickState trick)
        {
            var trickModel = this.GetModel<ITrickModel>();
            var scoreModel = this.GetModel<IScoreModel>();
            
            TrickInfo trickInfo = new TrickInfo(trick);
            trickModel.CurrentTricks.Value.Add(trickInfo);
            scoreModel.TotalScore.Value += trick.ScoreValue;
            
            Debug.Log($"添加技巧: {trick.TrickName}, 分数: {trick.ScoreValue}");
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

        private void OnPlayerLanded(PlayerLandedEvent evt)
        {
            ResetTrickScore();
            Debug.Log("玩家落地，清空技巧分数");
        }
        
        private void OnTrickPerformed(TrickPerformedEvent evt)
        {
            Debug.Log($"TrickSystem: 处理技巧执行事件 - {evt.TrickName}");
            
            var trickModel = this.GetModel<ITrickModel>();
            var scoreModel = this.GetModel<IScoreModel>();
            
            TrickState trick = playerController.stateMachine.TryGetState(evt.TrickName, StateLayer.Action) as TrickState;

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
            trickModel.TrickTimer.Value = trick.StateTotalDuration;
            
            // 更新分数
            scoreModel.TotalScore.Value += trick.ScoreValue;
            
            Debug.Log($"TrickSystem: 添加技巧 {evt.TrickName}, 分数: {trick.ScoreValue}, 总分: {scoreModel.TotalScore.Value}");
            Debug.Log($"TrickSystem: 技巧列表数量: {trickModel.CurrentTricks.Value.Count}");
        }
    }   
}
