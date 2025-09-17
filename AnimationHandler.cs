using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void SetJumping(bool isJumping)
    {
        animator.SetBool("isJumping", isJumping);
    }
    public void PlayDamagedAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("doDamaged");
        }
    }
    public void SetTransparency(float alpha)
    {
       
        if (spriteRenderer != null)
        {
            var c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
        }
    }

}
