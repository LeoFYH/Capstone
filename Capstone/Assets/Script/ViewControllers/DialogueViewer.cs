using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SkateGame
{
    /// <summary>
    /// 对话查看器
    /// 负责显示对话内容并处理交互
    /// </summary>
    public class DialogueViewer : ViewerControllerBase
    {
        [Header("对话设置")]
        public string NameForDialogue;
        
        [Header("UI组件")]
        public TextMeshProUGUI text;
        public Image image;
        
        [Header("对话列表")]
        public List<DialogueObj> dialogueList = new List<DialogueObj>();
        
        [Header("点击按钮")]
        public Button clickButton;
        
        // 私有变量
        private List<DialogueObj> ThisDialogueList;
        private int current = 0;
        private IDialogueSystem dialogueSystem;
        private IDialogueModel dialogueModel;
        
        protected override void InitializeController()
        {
            base.InitializeController();
            
            // 获取系统和模型
            dialogueSystem = this.GetSystem<IDialogueSystem>();
            dialogueModel = this.GetModel<IDialogueModel>();
            
            // 从system匹配对话列表
            ThisDialogueList = dialogueSystem.Match(NameForDialogue);
            
            if (ThisDialogueList == null || ThisDialogueList.Count == 0)
            {
                Debug.LogError($"DialogueViewer [{NameForDialogue}]: 未匹配到对话列表！");
                return;
            }
            
            // 设置按钮点击事件
            if (clickButton != null)
            {
                clickButton.onClick.AddListener(Click);
            }
            
            Debug.Log($"DialogueViewer [{NameForDialogue}]: 初始化完成，共 {ThisDialogueList.Count} 条对话");
        }
        
        protected override void OnRealTimeUpdate()
        {
            // 实时更新显示
            if (ThisDialogueList == null || ThisDialogueList.Count == 0)
                return;
            
            // 确保索引有效
            if (current < 0 || current >= ThisDialogueList.Count)
                current = 0;
            
            // 更新文本
            if (text != null)
            {
                text.text = ThisDialogueList[current].text;
            }
            
            // 更新图片
            if (image != null)
            {
                image.sprite = ThisDialogueList[current].image;
            }
        }
        
        /// <summary>
        /// 点击切换对话
        /// </summary>
        public void Click()
        {
            if (dialogueModel.TableForDialogue.ContainsKey(NameForDialogue))
            {
                int totalCount = dialogueModel.TableForDialogue[NameForDialogue].Count;
                current = (current + 1) % totalCount;
                Debug.Log($"DialogueViewer [{NameForDialogue}]: 切换到第 {current + 1}/{totalCount} 条");
            }
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (clickButton != null)
            {
                clickButton.onClick.RemoveListener(Click);
            }
        }
    }
}
