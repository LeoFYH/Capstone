using SkateGame;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    PlayerController playerController;

    void Start()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.IsGrounded())
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") < 0 ? true : Input.GetAxisRaw("Horizontal") > 0 ? false : spriteRenderer.flipX;
        }

    }

    public void OnLandingAnimationFinished()
    {
        animator.Play("oPlayer@Push");
    }
}
