using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI组件")]
    public TextMeshProUGUI scoreText; // 分数显示文本
    
    [Header("计分设置")]
    public int grindScorePerSecond = 50; // 每秒滑轨分数
    
    [Header("当前状态")]
    public int totalScore = 0; // 总分数
    public int currentCombo = 0; // 当前连击数
    public float grindTime = 0f; // 滑轨时间
    public float lastScoreChangeTime = 0f; // 最后一次分数变化时间
    public int lastScore = 0; // 上一次的分数

    [Header("战斗")]
    public float curretBulletTime; //现在的子弹时间剩余资源
    public float bulletTimeMax; //最大的子弹时间
    public float bulletTimeBoost; //额外的子弹时间上限
    public float bulletRecoverTime; //恢复到满的时间

    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("ScoreManager");
                    instance = go.AddComponent<ScoreManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // 更新滑轨时间计分
        if (grindTime > 0)
        {
            grindTime += Time.deltaTime;
            
            // 每0.01秒加1分
            if (grindTime >= 0.01f)
            {
                AddGrindScore(1);
                Debug.Log("滑轨加分 +1");
            }
        }
        
        // 检查分数是否在3秒内没有变化
        if (totalScore != lastScore)
        {
            lastScore = totalScore;
            lastScoreChangeTime = Time.time;
        }
        else if (Time.time - lastScoreChangeTime >= 3f && totalScore > 0)
        {
            // 3秒内分数没有变化，自动清零分数和combo
            ResetScore();
            Debug.Log("3秒内分数无变化，自动清零分数和combo");
        }
        // 检查combo是否在3秒内没有变化
       
    }

    // 添加滑轨分数
    public void AddGrindScore(float points)
    {
        int grindPoints = Mathf.RoundToInt(points);
        totalScore += grindPoints;
        UpdateUI();
    }

    // 添加特技分数
    public void AddTrickScore(int points = 1)
    {
        totalScore += points;
        currentCombo += 1;
        
        Debug.Log($"特技分数 +{points} (连击: {currentCombo})");
        UpdateUI();
    }

    // 开始滑轨计分
    public void StartGrindScoring()
    {
        grindTime = 0.001f; // 设置为一个很小的值，让Update开始计时
        Debug.Log("开始滑轨计分");
    }

    // 结束滑轨计分
    public void EndGrindScoring()
    {
        // 重置计时器
        grindTime = 0f;
        Debug.Log("结束滑轨计分");
    }

    // 重置分数
    public void ResetScore()
    {
        totalScore = 0;
        currentCombo = 0;
        grindTime = 0f;
        lastScore = 0;
        lastScoreChangeTime = Time.time;
        UpdateUI();
        Debug.Log("分数和combo已重置");
    }

    // 更新UI显示
    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {totalScore}\nCombo: {currentCombo}";
        }
    }

    // 获取当前总分
    public int GetTotalScore()
    {
        return totalScore;
    }

    // 获取当前连击数
    public int GetCurrentCombo()
    {
        return currentCombo;
    }
} 