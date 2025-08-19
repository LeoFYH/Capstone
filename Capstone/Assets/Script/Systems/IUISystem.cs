using QFramework;
using UnityEngine;

namespace SkateGame
{
    public interface IUISystem : ISystem
    {
        void UpdateScoreDisplay(int score);
        void UpdateTrickList();
        void ShowTrickNotification(string trickName);
    }

    public class UISystem : AbstractSystem, IUISystem
    {
        protected override void OnInit()
        {
            // 监听UI相关事件
            this.RegisterEvent<ScoreUpdatedEvent>(OnScoreUpdated);
            this.RegisterEvent<TrickPerformedEvent>(OnTrickPerformed);
        }

        public void UpdateScoreDisplay(int score)
        {
            // Debug.Log($"更新分数显示: {score}");
            // 实际的UI更新逻辑
        }

        public void UpdateTrickList()
        {
            var trickModel = this.GetModel<ITrickModel>();
            // Debug.Log($"更新技巧列表，当前技巧数量: {trickModel.CurrentTricks.Value.Count}");
            // 实际的UI更新逻辑
        }

        public void ShowTrickNotification(string trickName)
        {
            // Debug.Log($"显示技巧通知: {trickName}");
            // 实际的UI通知逻辑
        }

        private void OnScoreUpdated(ScoreUpdatedEvent evt)
        {
            UpdateScoreDisplay(evt.NewScore);
        }

        private void OnTrickPerformed(TrickPerformedEvent evt)
        {
            ShowTrickNotification(evt.TrickName);
            UpdateTrickList();
        }
    }
}
