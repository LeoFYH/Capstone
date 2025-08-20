using UnityEngine;
using UnityEngine.UI;

namespace SkateGame
{
    /// <summary>
    /// 技巧显示UI组件
    /// 用于显示单个技巧的名称和分数
    /// </summary>
    public class TrickDisplayUI : MonoBehaviour
    {
        [Header("UI组件")]
        public Text trickNameText;
        public Text scoreText;
        public Text comboText;
        
        [Header("动画设置")]
        public float fadeInTime = 0.3f;
        public float displayTime = 2f;
        public float fadeOutTime = 0.5f;
        
        private CanvasGroup canvasGroup;
        private float timer = 0f;
        private bool isFadingIn = true;
        private bool isDisplaying = false;
        private bool isFadingOut = false;
        
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            
            // 初始状态为透明
            canvasGroup.alpha = 0f;
        }
        
        private void Start()
        {
            // 开始淡入动画
            StartFadeIn();
        }
        
        private void Update()
        {
            if (isFadingIn)
            {
                timer += Time.deltaTime;
                float progress = timer / fadeInTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
                
                if (progress >= 1f)
                {
                    isFadingIn = false;
                    isDisplaying = true;
                    timer = 0f;
                }
            }
            else if (isDisplaying)
            {
                timer += Time.deltaTime;
                if (timer >= displayTime)
                {
                    isDisplaying = false;
                    isFadingOut = true;
                    timer = 0f;
                }
            }
            else if (isFadingOut)
            {
                timer += Time.deltaTime;
                float progress = timer / fadeOutTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
                
                if (progress >= 1f)
                {
                    Destroy(gameObject);
                }
            }
        }
        
        /// <summary>
        /// 设置技巧显示内容
        /// </summary>
        public void SetTrickInfo(string trickName, int score, int combo = 1)
        {
            if (trickNameText != null)
            {
                trickNameText.text = trickName;
            }
            
            if (scoreText != null)
            {
                scoreText.text = $"+{score}";
            }
            
            if (comboText != null && combo > 1)
            {
                comboText.text = $"x{combo}";
                comboText.gameObject.SetActive(true);
            }
            else if (comboText != null)
            {
                comboText.gameObject.SetActive(false);
            }
        }
        
        private void StartFadeIn()
        {
            isFadingIn = true;
            timer = 0f;
        }
        
        /// <summary>
        /// 立即销毁（用于外部调用）
        /// </summary>
        public void DestroyImmediate()
        {
            Destroy(gameObject);
        }
    }
}
