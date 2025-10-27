using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QFramework;

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
        
        [Header("选项按钮")]
        public Button[] buttons = new Button[3];
        
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
            
            // 设置选项按钮
            ClickWithOptions();
            
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
            
            // 更新选项按钮
            UpdateOptionsDisplay();
            
            EndDialogue();
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
        
        /// <summary>
        /// 设置选项按钮跳转
        /// </summary>
        public void ClickWithOptions()
        {
            if (ThisDialogueList == null || ThisDialogueList.Count == 0)
                return;
                
            var currentJumpingList = ThisDialogueList[current].indexForJump;
            
            for (int i = 0; i < 3; i++)
            {
                if (buttons != null && i < buttons.Length && buttons[i] != null)
                {
                    int index = i; // 捕获索引
                    buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = ThisDialogueList[current].buttonTexts[i];
                    buttons[i].onClick.RemoveAllListeners();
                    buttons[i].onClick.AddListener(() => 
                    {
                        if (currentJumpingList != null && index < currentJumpingList.Length)
                        {
                            current = currentJumpingList[index];
                            Debug.Log($"DialogueViewer [{NameForDialogue}]: 选项 {index + 1} 跳转到第 {current + 1} 条");
                        }
                    });
                }
            }
        }
        
        /// <summary>
        /// 更新选项按钮显示
        /// </summary>
        private void UpdateOptionsDisplay()
        {
            if (ThisDialogueList == null || current >= ThisDialogueList.Count)
                return;
                
            bool hasChoices = ThisDialogueList[current].hasChoices;
            
            // 根据hasChoices显示/隐藏普通按钮和选项按钮
            // 如果有选项，隐藏普通按钮，显示选项按钮
            // 如果没有选项，显示普通按钮，隐藏选项按钮
            if (clickButton != null)
            {
                clickButton.gameObject.SetActive(!hasChoices);
            }
            
            // 根据hasChoices显示/隐藏选项按钮
            for (int i = 0; i < 3; i++)
            {
                if (buttons != null && i < buttons.Length && buttons[i] != null)
                {
                    buttons[i].gameObject.SetActive(hasChoices);
                }
            }
            
            // 如果有选项，更新按钮跳转
            if (hasChoices)
            {
                ClickWithOptions();
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
        /// <summary>
        /// 结束对话，如果是最后一条则隐藏对话框
        /// </summary>
        public void EndDialogue()
        {
            if (dialogueModel.TableForDialogue.ContainsKey(NameForDialogue))
            {
                if (current == dialogueModel.TableForDialogue[NameForDialogue].Count - 1)
                {
                    // 隐藏对话框GameObject
                    this.gameObject.SetActive(false);
                    Debug.Log($"DialogueViewer [{NameForDialogue}]: 对话结束，已隐藏");
                }
            }
        }
    }
}
