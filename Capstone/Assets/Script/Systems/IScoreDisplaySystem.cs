using QFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace SkateGame
{
    public interface IScoreDisplaySystem : ISystem
    {
        void ShowTrickScore(string trickName, int score);
        void UpdateTrickList(List<TrickInfo> tricks);
        void UpdateTotalScore(int totalScore);
        void ShowCombo(int comboCount);
        void ClearTrickDisplay();
    }

    public class ScoreDisplaySystem : AbstractSystem, IScoreDisplaySystem
    {
        [Header("UI组件")]
        public Text scoreText;
        public Text trickListText;
        public Text notificationText;
        public Text comboText;
        public Text totalScoreText;
        
        [Header("技巧显示设置")]
        public Transform trickDisplayParent; // 技巧显示容器
        public GameObject trickDisplayPrefab; // 技巧显示预制体
        public float trickDisplayDuration = 3f; // 技巧显示持续时间
        
        private List<GameObject> activeTrickDisplays = new List<GameObject>();
        
        protected override void OnInit()
        {
            // 监听所有UI相关事件
            this.RegisterEvent<ScoreDisplayEvent>(OnScoreDisplay);
            this.RegisterEvent<TrickDisplayEvent>(OnTrickDisplay);
            this.RegisterEvent<TrickListDisplayEvent>(OnTrickListDisplay);
            this.RegisterEvent<ComboDisplayEvent>(OnComboDisplay);
            this.RegisterEvent<UIClearEvent>(OnUIClear);
            this.RegisterEvent<PlayerLandedEvent>(OnPlayerLanded);
            
            Debug.Log("ScoreDisplaySystem初始化完成 - 处理所有UI显示逻辑");
        }

        public void ShowTrickScore(string trickName, int score)
        {
            Debug.Log($"显示技巧分数: {trickName} +{score}");
            
            // 更新通知文本
            if (notificationText != null)
            {
                notificationText.text = $"{trickName} +{score}";
            }
            
            // 创建技巧显示
            CreateTrickDisplay(trickName, score);
        }

        public void UpdateTrickList(List<TrickInfo> tricks)
        {
            if (trickListText != null)
            {
                if (tricks.Count > 0)
                {
                    string trickList = string.Join("\n", tricks.Select((t, i) => $"{i + 1}. {t.trickName} (+{t.trickScore})"));
                    trickListText.text = $"Tricks:\n{trickList}";
                }
                else
                {
                    trickListText.text = "Tricks: None";
                }
            }
            
            // 更新连击显示
            ShowCombo(tricks.Count);
        }

        public void UpdateTotalScore(int totalScore)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {totalScore}";
            }
            
            if (totalScoreText != null)
            {
                totalScoreText.text = $"Total: {totalScore}";
            }
        }

        public void ShowCombo(int comboCount)
        {
            if (comboText != null)
            {
                if (comboCount > 1)
                {
                    comboText.text = $"Combo: x{comboCount}";
                }
                else
                {
                    comboText.text = "";
                }
            }
        }

        public void ClearTrickDisplay()
        {
            // 清理所有技巧显示
            foreach (var display in activeTrickDisplays)
            {
                if (display != null)
                {
                    UnityEngine.Object.Destroy(display);
                }
            }
            activeTrickDisplays.Clear();
            
            // 清空UI文本
            if (trickListText != null)
            {
                trickListText.text = "Tricks: None";
            }
            
            if (comboText != null)
            {
                comboText.text = "";
            }
            
            if (notificationText != null)
            {
                notificationText.text = "";
            }
        }

        // 事件处理方法
        private void OnScoreDisplay(ScoreDisplayEvent evt)
        {
            if (evt.TextComponent != null)
            {
                switch (evt.ScoreType)
                {
                    case ScoreDisplayType.TotalScore:
                        evt.TextComponent.text = $"Score: {evt.Value}";
                        break;
                    case ScoreDisplayType.ComboScore:
                        evt.TextComponent.text = $"Combo: x{evt.Value}";
                        break;
                    case ScoreDisplayType.TrickScore:
                        evt.TextComponent.text = $"+{evt.Value}";
                        break;
                }
            }
        }

        private void OnTrickDisplay(TrickDisplayEvent evt)
        {
            if (evt.TextComponent != null)
            {
                evt.TextComponent.text = $"Trick: {evt.TrickName}";
            }
            
            // 获取技巧信息并创建显示
            var trickScore = TrickScore.Instance;
            var lastTrick = trickScore.trickInfos.LastOrDefault();
            
            if (lastTrick != null)
            {
                ShowTrickScore(lastTrick.trickName, lastTrick.trickScore);
                UpdateTrickList(trickScore.trickInfos);
                UpdateTotalScore(trickScore.totalScore);
            }
        }

        private void OnTrickListDisplay(TrickListDisplayEvent evt)
        {
            if (evt.TextComponent != null)
            {
                if (evt.Tricks.Count > 0)
                {
                    string trickList = string.Join("\n", evt.Tricks.Select((t, i) => $"{i + 1}. {t.trickName} (+{t.trickScore})"));
                    evt.TextComponent.text = $"Tricks:\n{trickList}";
                }
                else
                {
                    evt.TextComponent.text = "Tricks: None";
                }
            }
            
            // 更新连击显示
            ShowCombo(evt.Tricks.Count);
        }

        private void OnComboDisplay(ComboDisplayEvent evt)
        {
            if (evt.TextComponent != null)
            {
                if (evt.ComboCount > 1)
                {
                    evt.TextComponent.text = $"Combo: x{evt.ComboCount}";
                }
                else
                {
                    evt.TextComponent.text = "";
                }
            }
        }

        private void OnUIClear(UIClearEvent evt)
        {
            switch (evt.ClearType)
            {
                case UIClearType.All:
                    ClearTrickDisplay();
                    break;
                case UIClearType.TrickList:
                    if (trickListText != null)
                    {
                        trickListText.text = "Tricks: None";
                    }
                    break;
                case UIClearType.Score:
                    if (scoreText != null)
                    {
                        scoreText.text = "Score: 0";
                    }
                    if (totalScoreText != null)
                    {
                        totalScoreText.text = "Total: 0";
                    }
                    break;
                case UIClearType.Notification:
                    if (notificationText != null)
                    {
                        notificationText.text = "";
                    }
                    break;
            }
        }

        private void OnPlayerLanded(PlayerLandedEvent evt)
        {
            Debug.Log("玩家落地，清空技巧显示");
            ClearTrickDisplay();
        }

        /// <summary>
        /// 创建技巧显示
        /// </summary>
        private void CreateTrickDisplay(string trickName, int score)
        {
            if (trickDisplayParent != null && trickDisplayPrefab != null)
            {
                GameObject display = UnityEngine.Object.Instantiate(trickDisplayPrefab, trickDisplayParent);
                
                // 获取TrickDisplayUI组件
                TrickDisplayUI trickDisplay = display.GetComponent<TrickDisplayUI>();
                if (trickDisplay != null)
                {
                    // 获取当前连击数
                    int comboCount = TrickScore.Instance.trickInfos.Count;
                    trickDisplay.SetTrickInfo(trickName, score, comboCount);
                }
                else
                {
                    // 如果没有TrickDisplayUI组件，使用旧的显示方式
                    Text trickNameText = display.transform.Find("TrickName")?.GetComponent<Text>();
                    Text scoreText = display.transform.Find("Score")?.GetComponent<Text>();
                    
                    if (trickNameText != null)
                    {
                        trickNameText.text = trickName;
                    }
                    
                    if (scoreText != null)
                    {
                        scoreText.text = $"+{score}";
                    }
                    
                    // 设置自动销毁
                    UnityEngine.Object.Destroy(display, trickDisplayDuration);
                }
                
                // 添加到活动列表
                activeTrickDisplays.Add(display);
                
                // 清理已销毁的显示
                activeTrickDisplays.RemoveAll(d => d == null);
            }
        }
    }
}
