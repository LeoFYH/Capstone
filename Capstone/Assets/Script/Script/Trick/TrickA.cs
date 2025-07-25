using UnityEngine;

public class TrickA : TrickBase
{
    public TrickA()
    {
        trickName = "TrickA";
        duration = 1.0f;
        scoreValue = 10;
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
        // TrickA 的特殊效果：改变颜色
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }
    }

    protected override void AddScore()
    {
        //ScoreManager.Instance.AddTrickScore(scoreValue);
    }
} 