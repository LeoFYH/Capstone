using UnityEngine;
using QFramework;

namespace SkateGame
{
    public class LevelSelectArea : ViewerControllerBase
    {
        public Canvas levelSelectCanvas;
        public string playerTag = "Player";
        
        protected override void InitializeController()
        {
            if (levelSelectCanvas != null)
            {
                levelSelectCanvas.gameObject.SetActive(false);
            }
            Debug.Log("LevelSelectArea 初始化");
        }
        
        protected override void OnRealTimeUpdate()
        {
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"触发器进入: {other.name}, Tag: {other.tag}");
            
            if (other.CompareTag(playerTag))
            {
                Debug.Log("检测到玩家进入");
                ShowCanvas();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"触发器离开: {other.name}");
            
            if (other.CompareTag(playerTag))
            {
                Debug.Log("检测到玩家离开");
                HideCanvas();
            }
        }
        
        private void ShowCanvas()
        {
            if (levelSelectCanvas != null)
            {
                levelSelectCanvas.gameObject.SetActive(true);
                Debug.Log("Canvas显示");
            }
        }
        
        private void HideCanvas()
        {
            if (levelSelectCanvas != null)
            {
                levelSelectCanvas.gameObject.SetActive(false);
                Debug.Log("Canvas隐藏");
            }
        }
    }
}

