using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using QFramework;

namespace SkateGame
{
    public class UiTrickList : ViewerControllerBase
    {
        public Text tricksText;     
        public Text gradeText;
        private ITrickListModel trickModel;

        protected override void InitializeController()
        {
            trickModel = this.GetModel<ITrickListModel>();
            
            if (trickModel != null)
            {
                RefreshUI();
                DisplayGrade();
            }

        }

        protected override void OnRealTimeUpdate()
        {
           RefreshUI();
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