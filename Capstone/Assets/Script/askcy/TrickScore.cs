using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using QFramework;
using SkateGame;

public class TrickScore : ViewerControllerBase
{
    private static TrickScore instance;
    public static TrickScore Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<TrickScore>();
                if (instance == null)
                {
                    GameObject go = new GameObject("TrickScore");
                    instance = go.AddComponent<TrickScore>();
                }
            }
            return instance;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<TrickInfo> trickInfos = new List<TrickInfo>();
    public int totalScore;
    private bool isResetting = false;
   
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

    public void AddTrickScore(TrickState trickBase)
    {
        TrickInfo newTrick = new TrickInfo(trickBase);
        trickInfos.Add(newTrick);
        totalScore += trickBase.ScoreValue;
        
        Debug.Log($"添加技巧: {trickBase.TrickName}, 分数: {trickBase.ScoreValue}");
        
        // 直接更新模型，不再发送事件
        // UIController会监听模型变化自动更新UI
        var trickModel = this.GetModel<ITrickModel>();
        var scoreModel = this.GetModel<IScoreModel>();
        
        trickModel.CurrentTricks.Value.Add(newTrick);
        scoreModel.TotalScore.Value = totalScore;
        trickModel.CurrentTrickName.Value = trickBase.TrickName;
    }
    
    public void RemoveTrickScore(TrickState trickBase)
    {
        // Find and remove the specific trick info
        TrickInfo trickToRemove = trickInfos.Find(trick => trick.trickBase == trickBase);
        if (trickToRemove != null)
        {
            trickInfos.Remove(trickToRemove);
            totalScore -= trickBase.ScoreValue;
            
            // 直接更新模型，不再发送事件
            var trickModel = this.GetModel<ITrickModel>();
            var scoreModel = this.GetModel<IScoreModel>();
            
            trickModel.CurrentTricks.Value.Remove(trickToRemove);
            scoreModel.TotalScore.Value = totalScore;
        }
    }

    public void ResetTrickScore()
    {
        trickInfos.Clear();
        totalScore = 0;
        
        // 直接更新模型，不再发送事件
        var trickModel = this.GetModel<ITrickModel>();
        var scoreModel = this.GetModel<IScoreModel>();
        
        trickModel.CurrentTricks.Value.Clear();
        scoreModel.TotalScore.Value = 0;
        trickModel.CurrentTrickName.Value = "";
    }

    // 移除OnPlayerLanded方法，让系统层处理落地逻辑
    // public void OnPlayerLanded()
    // {
    //     if (!isResetting && trickInfos.Count > 0)
    //     {
    //         Debug.Log("=== 玩家落地，技巧列表 ===");
    //         PrintAllTricks();
    //         Debug.Log("5秒后清零技巧列表...");
    //         
    //         // 发送玩家落地事件
    //         this.SendEvent<PlayerLandedEvent>(new PlayerLandedEvent());
    //         
    //         // 开始协程，等待5秒后清零
    //         StartCoroutine(ResetAfterDelay(5f));
    //     }
    // }

    private IEnumerator ResetAfterDelay(float delay)
    {
        isResetting = true;
        yield return new WaitForSeconds(delay);
        
        Debug.Log("技巧列表已清零");
        ResetTrickScore();
        isResetting = false;
    }

    // 打印所有技巧信息
    public void PrintAllTricks()
    {
        Debug.Log("=== 技巧列表 ===");
        for (int i = 0; i < trickInfos.Count; i++)
        {
            TrickInfo info = trickInfos[i];
            Debug.Log($"{i + 1}. {info.trickName} - 分数: {info.trickScore}");
        }
        Debug.Log($"总技巧分数: {totalScore}");
        Debug.Log("================");
    }
    
    // 实现ViewerControllerBase的抽象方法
    protected override void OnRealTimeUpdate()
    {
        // TrickScore不需要实时更新逻辑
        // 所有逻辑都通过事件处理
    }
}

public class TrickInfo
{
    public ActionStateBase trickBase;
    public string trickName;
    public int trickScore;

    public TrickInfo(TrickState trickBase)
    {
        this.trickBase = trickBase;
        this.trickName = trickBase.TrickName;
        this.trickScore = trickBase.ScoreValue;
    }
}