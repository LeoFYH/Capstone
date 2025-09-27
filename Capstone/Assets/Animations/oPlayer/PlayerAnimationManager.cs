using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLandingAnimationFinished()
    {
        animator.Play("oPlayer@Push");
    }

    public void LockAnimation()
    {
        animator.SetBool("isAnimationLocked", true);
    }

    public void UnlockAnimation()
    {
        animator.SetBool("isAnimationLocked", false);
    }
}
