using System;
using UnityEngine;

namespace SkateGame
{
    /// <summary>
    /// 对话对象，包含单个对话的文本和图片
    /// </summary>
    [Serializable]
    public class DialogueObj
    {
        [Header("对话内容")]
        [TextArea(3, 10)]
        public string text;
        
        [Header("对话图片")]
        public Sprite image;
        
        public DialogueObj()
        {
            text = "";
            image = null;
        }
        
        public DialogueObj(string text, Sprite image)
        {
            this.text = text;
            this.image = image;
        }
    }
}

