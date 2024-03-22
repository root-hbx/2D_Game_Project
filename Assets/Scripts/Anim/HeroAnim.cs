using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnim : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public enum HeroAnimCmd
    {
        TurnRight,
        TurnLeft,
        StopHorizontal,
        Jumping,
        Falling,
        StopVertical,
    }

    // Update is called once per frame
    public void SetAnim(HeroAnimCmd heroCmd)
    {
        switch (heroCmd)
        {
            case HeroAnimCmd.TurnRight:
                spriteRenderer.flipX = false;
                animator.SetBool("isRunning", true);
                break;
            case HeroAnimCmd.TurnLeft:
                spriteRenderer.flipX = true;
                animator.SetBool("isRunning", true);
                break;
            case HeroAnimCmd.StopHorizontal:
                animator.SetBool("isRunning", false);
                break;
            case HeroAnimCmd.Jumping:
                animator.SetBool("isJumping", true);
                break;
            case HeroAnimCmd.Falling:
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
                break;
            case HeroAnimCmd.StopVertical:
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
                break;
        }
    }

}
