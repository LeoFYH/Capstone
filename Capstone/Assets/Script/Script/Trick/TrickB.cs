using UnityEngine;

public class TrickB : TrickBase
{
    public TrickB()
    {
        trickName = "TrickB";
        duration = 3.0f;
        scoreValue = 15;
    }

    public override void PerformTrick(PlayerScript player)
    {
        base.PerformTrick(player);
        PlayAnimation(player);
        PlayEffects(player);
        AddScore();
    }

    public override void Exit(PlayerScript player)
    {
       
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
        Debug.Log("TrickB 退出，重置颜色");
    }

    protected override void PlayAnimation(PlayerScript player)
    {
       
    }

    protected override void PlayEffects(PlayerScript player)
    {
        // TrickB 的特殊效果：改变颜色
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.blue;
        }
    }

    protected override void AddScore()
    {
        // 直接调用TrickScore，不需要通过PlayerScript
        TrickScore.Instance.AddTrickScore(this);
        
        Debug.Log($"TrickB 完成！分数: {scoreValue}");
    }
} 