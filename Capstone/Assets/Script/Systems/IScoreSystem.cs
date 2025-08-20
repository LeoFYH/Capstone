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
            Debug.Log($"技巧执行: {evt.TrickName}");
        }
    }
}
