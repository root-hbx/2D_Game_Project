using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    public IInput input = new ActualInput();
    private HeroAnim heroAnim;
    private GameObject indicator;

    const float kMoveAcceleration = 100.0f;
    const float kMaxMoveSpeed = 10.0f;
    const float kJumpForce = 30.0f;

    Rigidbody2D rb;
    const float kFallMultiplier = 8f;
    const float kLowJumpMultiplier = 10f;

    const float kCoyoteTime = 0.1f;
    float leftGroundCoyoteTime = 0;

    bool IsGrounded
    {
        get
        {
            return Physics2D.OverlapCircle((Vector2)transform.position, 0.25f, 1 << LayerMask.NameToLayer("Ground"));
        }
    }

    void Start()
    {
        heroAnim = GetComponent<HeroAnim>();
        Debug.Assert(heroAnim != null, "HeroAnim not found");
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, "Rigidbody2D not found");
        if (input is ActualInput)
        {
            indicator = Instantiate(Resources.Load<GameObject>("Prefabs/Indicator"),
            transform.position + new Vector3(0, GetComponent<Renderer>().bounds.size.y + 1, 0),
            Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        BetterMovement();
        UpdateJump();
        BetterJump();
        UpdateCoyoTime();

        input.ConsumeFrame();
        if (indicator != null)
        {
            indicator.transform.position = transform.position + new Vector3(0, GetComponent<Renderer>().bounds.size.y + 1, 0);
        }
    }

    void UpdateMovement()
    {
        int moveDir = 0;
        if (input.GetKey(InputKey.A))
        {
            moveDir = -1;
            heroAnim.FlipX = true;
        }
        else if (input.GetKey(InputKey.D))
        {
            moveDir = 1;
            heroAnim.FlipX = false;
        }
        else
        {
            heroAnim.IsRuning = false;
        }

        if (moveDir != 0)
        {
            rb.AddForce(new Vector2(moveDir * kMoveAcceleration, 0));
            if (!heroAnim.IsJumping)
            {
                heroAnim.IsRuning = true;
            }
        }
    }

    void BetterMovement()
    {
        if (Mathf.Abs(rb.velocity.x) > kMaxMoveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * kMaxMoveSpeed, rb.velocity.y);
        }
    }

    void UpdateJump()
    {
        if (input.GetKey(InputKey.W))
        {
            if (IsGrounded || leftGroundCoyoteTime + kCoyoteTime > Time.time)
            {
                Debug.Log("Jump");
                heroAnim.IsJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, kJumpForce);
                StartCoroutine(nameof(StopJumpAnime));
            }
        }
    }

    IEnumerator StopJumpAnime()
    {
        yield return new WaitForSeconds(0.2f);
        while (rb.velocity.y > 0)
        {
            yield return new WaitForEndOfFrame();
        }
        heroAnim.IsJumping = false;
    }

    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += kFallMultiplier * Physics2D.gravity.y * Time.smoothDeltaTime * Vector2.up;
        }
        else if (rb.velocity.y > 0 && !input.GetKey(InputKey.W))
        {
            rb.velocity += kLowJumpMultiplier * Physics2D.gravity.y * Time.smoothDeltaTime * Vector2.up;
        }
    }

    void UpdateCoyoTime()
    {
        if (IsGrounded)
        {
            leftGroundCoyoteTime = Time.time;
        }
    }
}
