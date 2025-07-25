using UnityEngine;

public class TrickB : TrickBase
{
    public TrickB()
    {
        trickName = "TrickB";
        duration = 1.5f;
        scoreValue = 15;
    }

    public override void PerformTrick(PlayerScript player)
    {
        base.PerformTrick(player);
        PlayAnimation(player);
        PlayEffects(player);
        AddScore();
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
        //ScoreManager.Instance.AddTrickScore(scoreValue);
    }
} 