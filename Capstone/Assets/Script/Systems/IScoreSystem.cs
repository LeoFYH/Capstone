using QFramework;
using UnityEngine;

namespace SkateGame
{
    public interface IScoreSystem : ISystem
    {
        void AddScore(int score);
        void ResetScore();
        void UpdateCombo(int combo);
    }

    public class ScoreSystem : AbstractSystem, IScoreSystem
    {
        protected override void OnInit()
        {
            // 监听分数相关事件
            this.RegisterEvent<TrickPerformedEvent>(OnTrickPerformed);
            this.RegisterEvent<PlayerLandedEvent>(OnPlayerLanded);
        }

        public void AddScore(int score)
        {
            var scoreModel = this.GetModel<IScoreModel>();
            scoreModel.TotalScore.Value += score * scoreModel.ComboMultiplier.Value;
            
            // 不再发送事件，直接更新模型
            // UIController会监听模型变化自动更新UI
        }

        public void ResetScore()
        {
            var scoreModel = this.GetModel<IScoreModel>();
            scoreModel.TotalScore.Value = 0;
            scoreModel.ComboMultiplier.Value = 1;
            
            // 不再发送事件，直接更新模型
            // UIController会监听模型变化自动更新UI
        }

        public void UpdateCombo(int combo)
        {
            var scoreModel = this.GetModel<IScoreModel>();
            scoreModel.ComboMultiplier.Value = combo;
        }

        private void OnTrickPerformed(TrickPerformedEvent evt)
        {
            // 技巧执行时的分数逻辑
            var scoreModel = this.GetModel<IScoreModel>();
            
            // 获取当前等级并升级
            char currentGrade = scoreModel.CurrentGrade.Value;
            char newGrade = UpgradeGrade(currentGrade);
            
            // 更新当前等级
            scoreModel.CurrentGrade.Value = newGrade;
            
            Debug.Log($"技巧执行: {evt.TrickName}, 等级从 {currentGrade} 升级到 {newGrade}");
        }
        
        private void OnPlayerLanded(PlayerLandedEvent evt)
        {
            // 玩家着陆时重置等级为默认值
            //var scoreModel = this.GetModel<IScoreModel>();
            //char previousGrade = scoreModel.CurrentGrade.Value;
            //scoreModel.CurrentGrade.Value = 'D'; // 重置为默认等级
            
            //Debug.Log($"玩家着陆，等级从 {previousGrade} 重置为 D");
        }
        
        private char UpgradeGrade(char currentGrade)
        {
            // 等级升级逻辑：D -> C -> B -> A -> S
            switch (currentGrade)
            {
                case 'D': return 'C';
                case 'C': return 'B';
                case 'B': return 'A';
                case 'A': return 'S';
                case 'S': return 'S'; // S级是最高等级，不再升级
                default: return 'D'; // 默认从D开始
            }
        }
    }
}
