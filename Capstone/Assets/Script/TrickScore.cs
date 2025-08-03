using UnityEngine;
using System.Collections.Generic;

public class TrickScore : MonoBehaviour
{
    private static TrickScore instance;
    public static TrickScore Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TrickScore>();
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

    public void AddTrickScore(TrickBase trickBase)
    {
        trickInfos.Add(new TrickInfo(trickBase));
        totalScore += trickBase.scoreValue;
        Debug.Log($"添加技巧: {trickBase.trickName}, 分数: {trickBase.scoreValue}");
    }
    
    public void RemoveTrickScore(TrickBase trickBase)
    {
        // Find and remove the specific trick info
        TrickInfo trickToRemove = trickInfos.Find(trick => trick.trickBase == trickBase);
        if (trickToRemove != null)
        {
            trickInfos.Remove(trickToRemove);
            totalScore -= trickBase.scoreValue;
        }
    }

    public void ResetTrickScore()
    {
        trickInfos.Clear();
        totalScore = 0;
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
}

public class TrickInfo
{
    public TrickBase trickBase;
    public string trickName;
    public int trickScore;

    public TrickInfo(TrickBase trickBase)
    {
        this.trickBase = trickBase;
        this.trickName = trickBase.trickName;
        this.trickScore = trickBase.scoreValue;
    }
}