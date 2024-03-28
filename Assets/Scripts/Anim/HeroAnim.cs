using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HeroAnim : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    public bool IsRunning
    {
        get { return animator.GetBool("isRunning"); }
        set { animator.SetBool("isRunning", value); }
    }

    public bool IsJumping
    {
        get { return animator.GetBool("isJumping"); }
        set { animator.SetBool("isJumping", value); }
    }

    public bool FlipX
    {
        get { return spriteRenderer.flipX; }
        set { spriteRenderer.flipX = value; }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        Assert.IsNotNull(animator, "Animator not found");
        spriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(spriteRenderer, "SpriteRenderer not found");
    }
}
