using UnityEngine;
using TMPro;
using System.Collections.Generic;
using QFramework;
using UnityEngine.UI;

namespace SkateGame
{
    /// <summary>
    /// UI控制器
    /// 使用TextMeshPro显示技巧的名称、数量和单个对应分数
    /// </summary>
    public class UIController : ViewerControllerBase
    {
        private IPlayerModel playerModel;
        [Header("TextMeshPro UI组件")]
        public Text trickInfoText;      // 技巧信息显示（包含名称、数量、分数）
        public Text aimTimeText;        // 瞄准时间上限显示
        public Text gradeText;          // 等级显示


        protected override void Start()
        {
            base.Start();
        }
        protected override void InitializeController()
        {
            
            // 获取玩家参数
            playerModel = this.GetModel<IPlayerModel>();
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
            scoreModel.CurrentGrade.Register(OnGradeChanged);
            
            // Display current values on initialization
            OnTrickListChanged(trickModel.CurrentTricks.Value);
            OnTrickNameChanged(trickModel.CurrentTrickName.Value);
            OnScoreChanged(scoreModel.TotalScore.Value);
            OnGradeChanged(scoreModel.CurrentGrade.Value);
            
            // Initialize aim time display
            InitializeAimTimeDisplay();
        }
        
        protected override void OnRealTimeUpdate()
        {
            // 更新瞄准时间显示
            UpdateAimTimeDisplay();
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
        
        private void OnGradeChanged(char newGrade)
        {
            Debug.Log($"UIController: Grade changed - {newGrade}");
            
            // Update grade display
            if (gradeText != null)
            {
                gradeText.text = $"Rank: {newGrade}";
                Debug.Log($"UIController: Updated gradeText - {newGrade}");
            }
            else
            {
                Debug.LogWarning("UIController: gradeText is null!");
            }
        }

        
        // 初始化瞄准时间显示
        private void InitializeAimTimeDisplay()
        {
            if (aimTimeText != null)
            {  
                    float maxAimTime = playerModel.MaxAimTime.Value;
                    aimTimeText.text = $"Aim Time: {maxAimTime:F1}s";
                    Debug.Log($"UIController: 初始化瞄准时间显示 - {maxAimTime}秒");
               
                    aimTimeText.text = "Aim Time: 3.0s";
                    Debug.LogWarning("UIController: 无法找到InputController，使用默认瞄准时间");
               
            }
        }

        // 更新瞄准时间显示
        private void UpdateAimTimeDisplay()
        {
            if (aimTimeText != null)
            {
                // 获取InputController来获取当前瞄准状态
                InputController playerController = Object.FindFirstObjectByType<InputController>();
                if (playerController != null)
                {
                    if (playerModel.IsAiming.Value)
                    {
                        // 计算剩余瞄准时间
                        float remainingTime = playerModel.MaxAimTime.Value - playerController.GetAimTimer();
                        remainingTime = Mathf.Max(0, remainingTime);
                        aimTimeText.text = $"瞄准中... 剩余: {remainingTime:F1}秒";
                    }
                    else
                    {
                        // 显示当前瞄准时间上限，如果超过基础值则显示奖励信息
                        if (playerModel.MaxAimTime.Value > playerController.baseMaxAimTime)
                        {
                            float bonus = playerModel.MaxAimTime.Value - playerController.baseMaxAimTime;
                            aimTimeText.text = $"Aim Time: {playerModel.MaxAimTime.Value:F1}s (+{bonus:F1}s)";
                        }
                        else
                        {
                            aimTimeText.text = $"Aim Time: {playerModel.MaxAimTime.Value:F1}s";
                        }
                    }
                }
            }
        }

        protected override void OnDestroy()
        {
            // 调用基类的OnDestroy
            base.OnDestroy();
        }
    }
}
