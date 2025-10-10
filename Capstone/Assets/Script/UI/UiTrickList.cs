using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Linq;
using QFramework;

namespace SkateGame
{
    public class UiTrickList : ViewerControllerBase
    {
        public Text tricksText;     
        public Text gradeText;
        private ITrickListModel trickModel;
        private IPlayerModel playerModel;
        private ITrickSystem trickSystem;
        private int sum = 0;

        protected override void InitializeController()
        {
            trickModel = this.GetModel<ITrickListModel>();
            playerModel = this.GetModel<IPlayerModel>();
            trickSystem = this.GetSystem<ITrickSystem>();
            
            if (trickModel != null)
            {
                RefreshUI();
                DisplayGrade();
                
                // 注册事件监听
                this.RegisterEvent<TrickListChangedEvent>(OnTrickListChanged)
                    .UnRegisterWhenGameObjectDestroyed(gameObject);
                
            }
        }
        
        private void OnTrickListChanged(TrickListChangedEvent evt)
        {
            
            RefreshUI();
        }

        protected override void OnRealTimeUpdate()
        {
            // 检测落地，清空技巧列表
            if (playerModel != null && playerModel.IsGrounded.Value)
            {
                
                    tricksText.text = "";
                    sum += trickSystem.SumOfScore();
                    gradeText.text = sum.ToString();
                    trickSystem.RemoveAllTricks();

            }
        }


        public void RefreshUI()
        {
            if (tricksText == null || trickModel == null || trickModel.TrickList.Value.Count == 0) return;
            
            StringBuilder sb = new StringBuilder();
            
            foreach (var trick in trickModel.TrickList.Value)
            {
                sb.Append(trick.TrickName);
                sb.Append("   ");
                sb.Append(trick.ScoreValue);
                sb.Append("\n\n");  // 两个换行符，形成空行
            }
            
            tricksText.text = sb.ToString();
            
        }

        public void DisplayGrade()
        {
            if (gradeText == null || trickModel == null) return;
            
            gradeText.text = trickModel.Grade.Value.ToString();
        }
    }
}