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
}
