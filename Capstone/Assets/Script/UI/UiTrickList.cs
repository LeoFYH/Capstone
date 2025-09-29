using UnityEngine;
using UnityEngine.UI;
using System.Text;
using QFramework;

namespace SkateGame
{
    public class UiTrickList : ViewerControllerBase
    {
        private Text tricksText;     
        private Text gradeText;
        private ITrickListModel trickModel;

        protected override void InitializeController()
        {
            // 获取UI组件
            tricksText = GetComponentInChildren<Text>();
            gradeText = GetComponentInChildren<Text>();
            
            // 获取模型
            trickModel = this.GetModel<ITrickListModel>();
            
            // 监听模型变化
            if (trickModel != null)
            {
                trickModel.TrickList.Register(OnTrickListChanged);
                trickModel.Grade.Register(OnGradeChanged);
            }
        }

        protected override void OnRealTimeUpdate()
        {
           
        }

        private void OnTrickListChanged(System.Collections.Generic.List<TrickState> tricks)
        {
            RefreshUI();
        }

        private void OnGradeChanged(char grade)
        {
            DisplayGrade();
        }

        public void RefreshUI()
        {
            if (tricksText == null || trickModel == null) return;
            
            StringBuilder sb = new StringBuilder();

            foreach (var trick in trickModel.TrickList.Value)
            {
                sb.Append(trick.TrickName);
                sb.Append("   ");
                sb.Append(trick.ScoreValue);
                sb.Append("\n");  
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