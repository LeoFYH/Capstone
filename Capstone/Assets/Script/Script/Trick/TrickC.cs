using UnityEngine;

public class TrickC : TrickBase
{
    public TrickC()
    {
        trickName = "TrickC";
        duration = 3f;
        scoreValue = 300;
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
        Debug.Log("TrickC 退出，重置颜色");
    }

    protected override void PlayAnimation(PlayerScript player)
    {

    }

    protected override void PlayEffects(PlayerScript player)
    {
        // TrickC 的特殊效果：改变颜色
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow;
        }
    }

    protected override void AddScore()
    {
        ScoreManager.Instance.AddTrickScore(scoreValue);
    }
}
