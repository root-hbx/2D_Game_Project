using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MoveBehaviour : MonoBehaviour
{
    HeroAnim heroAnim;
    GameObject indicator;
    Rigidbody2D rigidBody;

    public IInput input = new ActualInput();

    const float kMoveAcceleration = 80.0f;
    const float kMaxMoveSpeed = 10.0f;
    const float kJumpForce = 37.0f;     // Each deep jump is 7 units high
    const float kFallMultiplier = 8f;
    const float kLowJumpMultiplier = 10f;
    const float kCoyoteTime = 0.15f;
    const float kConsumeTime = 0.05f;
    float lastGroundTime = 0;
    float lastJumpTime = 0;

    bool IsGrounded => Physics2D.OverlapCircle((Vector2)transform.position, 0.25f, 1 << LayerMask.NameToLayer("Ground"));

    void Start()
    {
        heroAnim = GetComponent<HeroAnim>();
        Assert.IsNotNull(heroAnim, "HeroAnim not found");
        rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rigidBody, "Rigidbody2D not found");
    }

    void FixedUpdate()
    {
        UpdateMovement();
        BetterMovement();
        UpdateJump();
        BetterJump();
        UpdateGroundTime();

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
            rigidBody.AddForce(new Vector2(moveDir * kMoveAcceleration, 0), ForceMode2D.Impulse);
            if (!heroAnim.IsJumping)
            {
                heroAnim.IsRuning = true;
            }
        }
    }

    void BetterMovement()
    {
        if (Mathf.Abs(rigidBody.velocity.x) > kMaxMoveSpeed)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * kMaxMoveSpeed, rigidBody.velocity.y);
        }
    }

    void UpdateJump()
    {
        if (input.GetKey(InputKey.W))
        {
            if (lastJumpTime + kConsumeTime < Time.time && Time.time < lastGroundTime + kCoyoteTime)
            {
                heroAnim.IsJumping = true;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, kJumpForce);
                StartCoroutine(nameof(StopJumpAnime));
            }
        }
    }

    IEnumerator StopJumpAnime()
    {
        yield return new WaitForSeconds(0.2f);
        while (rigidBody.velocity.y > 0)
        {
            yield return new WaitForEndOfFrame();
        }
        heroAnim.IsJumping = false;
    }

    void BetterJump()
    {
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += kFallMultiplier * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;
        }
        else if (rigidBody.velocity.y > 0 && !input.GetKey(InputKey.W))
        {
            rigidBody.velocity += kLowJumpMultiplier * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;
        }
    }

    void UpdateGroundTime()
    {
        if (IsGrounded)
        {
            lastGroundTime = Time.time;
        }
        else
        {
            lastJumpTime = Time.time;
        }
    }

    public void AddIndicator()
    {
        indicator = Instantiate(Resources.Load<GameObject>("Prefabs/Indicator"),
            transform.position + new Vector3(0, GetComponent<Renderer>().bounds.size.y + 1, 0),
            Quaternion.identity, transform);
    }
}
