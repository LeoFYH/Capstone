using UnityEngine;
using TMPro;
using System.Collections.Generic;
using QFramework;

namespace SkateGame
{
    /// <summary>
    /// UI控制器
    /// 使用TextMeshPro显示技巧的名称、数量和单个对应分数
    /// </summary>
    public class UIController : ViewerControllerBase
    {
            [Header("TextMeshPro UI组件")]
    public TextMeshProUGUI trickInfoText;      // 技巧信息显示（包含名称、数量、分数）
        
        protected override void InitializeController()
        {
            Debug.Log("UIController initialized - Using TextMeshPro to display trick information");
            
            // Check if models are correctly obtained
            var scoreModel = this.GetModel<IScoreModel>();
            var trickModel = this.GetModel<ITrickModel>();
            
            if (scoreModel == null)
            {
                Debug.LogError("UIController: Cannot get IScoreModel!");
                return;
            }
            
            if (trickModel == null)
            {
                Debug.LogError("UIController: Cannot get ITrickModel!");
                return;
            }
            
            Debug.Log($"UIController: Successfully obtained models - ScoreModel: {scoreModel}, TrickModel: {trickModel}");
            
            // Listen to model changes
            trickModel.CurrentTricks.Register(OnTrickListChanged);
            trickModel.CurrentTrickName.Register(OnTrickNameChanged);
            scoreModel.TotalScore.Register(OnScoreChanged);
            
            // Display current values on initialization
            OnTrickListChanged(trickModel.CurrentTricks.Value);
            OnTrickNameChanged(trickModel.CurrentTrickName.Value);
            OnScoreChanged(scoreModel.TotalScore.Value);
        }
        
        protected override void OnRealTimeUpdate()
        {
            // UIController不处理实时更新逻辑
            // 所有逻辑都在系统层处理
        }
        
        private void OnTrickListChanged(List<TrickInfo> tricks)
        {
            Debug.Log($"UIController: Trick list changed - Count: {tricks.Count}");
            
            // Update trick information display
            if (trickInfoText != null)
            {
                string displayText = "";
                var scoreModel = this.GetModel<IScoreModel>();
                
                if (tricks.Count > 0)
                {
                    // Display each trick on a separate line
                    for (int i = 0; i < tricks.Count; i++)
                    {
                        var trick = tricks[i];
                        displayText += $"{i + 1}. {trick.trickName} - Score: {trick.trickScore}\n";
                    }
                    
                    // Add total information
                    displayText += $"Total Tricks: {tricks.Count}\n";
                    displayText += $"Total Score: {scoreModel.TotalScore.Value}";
                }
                else
                {
                    displayText = "No tricks performed\nTotal Score: 0";
                }
                
                trickInfoText.text = displayText;
                Debug.Log($"UIController: Updated trickInfoText - {displayText}");
            }
            else
            {
                Debug.LogWarning("UIController: trickInfoText is null!");
            }
        }
        
        private void OnTrickNameChanged(string trickName)
        {
            Debug.Log($"UIController: Trick name changed - {trickName}");
            
            // Current trick name is now handled in OnTrickListChanged
            // This method can be used for additional notifications if needed
        }
        
        private void OnScoreChanged(int newScore)
        {
            Debug.Log($"UIController: Score changed - {newScore}");
            // Force update the display when score changes
            var trickModel = this.GetModel<ITrickModel>();
            OnTrickListChanged(trickModel.CurrentTricks.Value);
        }
        
        protected override void OnDestroy()
        {
            // 调用基类的OnDestroy
            base.OnDestroy();
        }
    }
}
