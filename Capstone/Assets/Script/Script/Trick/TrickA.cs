using UnityEngine;

public class TrickA : TrickBase
{
    public TrickA()
    {
        trickName = "TrickA";
        duration = 0.3f;
        scoreValue = 10;
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
        Debug.Log("TrickA 退出，重置颜色");
    }

    protected override void PlayAnimation(PlayerScript player)
    {
        // TrickA 的特殊动画：快速旋转
        player.transform.Rotate(0, 0, 360f);
    }

    protected override void PlayEffects(PlayerScript player)
    {
        // TrickA 的特殊效果：改变颜色
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }
    }

    protected override void AddScore()
    {
        // 直接调用TrickScore，不需要通过PlayerScript
        TrickScore.Instance.AddTrickScore(this);
        
        Debug.Log($"TrickA 完成！分数: {scoreValue}");
    }
} 