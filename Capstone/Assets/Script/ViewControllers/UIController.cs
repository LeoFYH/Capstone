using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using QFramework;

namespace SkateGame
{
    /// <summary>
    /// UI控制器
    /// 负责UI显示和更新
    /// 监听模型变化并更新UI
    /// </summary>
    public class UIController : ViewerControllerBase
    {
        [Header("UI组件")]
        public Text scoreText;
        public Text trickListText;
        public Text notificationText;
        
        protected override void InitializeController()
        {
            Debug.Log("UI控制器初始化完成");
            
            // 监听模型变化
            this.GetModel<IScoreModel>().TotalScore.Register(OnScoreChanged);
            this.GetModel<ITrickModel>().CurrentTricks.Register(OnTrickListChanged);
            
            // 监听事件
            this.RegisterEvent<TrickPerformedEvent>(OnTrickPerformed);
        }
        
        protected override void OnRealTimeUpdate()
        {
            // UI的实时更新逻辑（如果需要的话）
            // 大部分UI更新通过事件和模型监听完成
        }
        
        private void OnScoreChanged(int newScore)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {newScore}";
            }
        }
        
        private void OnTrickListChanged(List<TrickInfo> tricks)
        {
            if (trickListText != null)
            {
                string trickList = string.Join(", ", tricks.ConvertAll(t => t.trickName));
                trickListText.text = $"Tricks: {trickList}";
            }
        }
        
        private void OnTrickPerformed(TrickPerformedEvent evt)
        {
            if (notificationText != null)
            {
                notificationText.text = $"Trick: {evt.TrickName}";
                // 可以添加动画效果
            }
        }
        
        protected override void OnDestroy()
        {
            // 清理事件监听
            this.UnRegisterEvent<TrickPerformedEvent>(OnTrickPerformed);
            
            // 调用基类的OnDestroy
            base.OnDestroy();
        }
    }
}
