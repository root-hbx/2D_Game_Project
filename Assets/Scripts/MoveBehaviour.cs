using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    public IInput input = new ActualInput();
    private HeroAnim heroAnim;

    readonly float moveSpeed = 12.0f;
    readonly float jumpStrength = 15.0f;

    bool attemptJump = false;
    bool isJumping = false;

    void Start()
    {
        heroAnim = GetComponent<HeroAnim>();
        Debug.Assert(heroAnim != null, "HeroAnim not found");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            heroAnim.SetAnim(HeroAnim.HeroAnimCmd.StopVertical);
        }
    }

    void FixedUpdate()
    {
        if (attemptJump)
        {
            if (!isJumping)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
                isJumping = true;
                heroAnim.SetAnim(HeroAnim.HeroAnimCmd.Jumping);
            }
            attemptJump = false;
        }
    }

    void UpdateMovement()
    {
        if (input.GetKey(InputKey.A))
        {
            transform.Translate(moveSpeed * Time.smoothDeltaTime * Vector3.left);
            heroAnim.SetAnim(HeroAnim.HeroAnimCmd.TurnLeft);
        }
        else if (input.GetKey(InputKey.D))
        {
            transform.Translate(moveSpeed * Time.smoothDeltaTime * Vector3.right);
            heroAnim.SetAnim(HeroAnim.HeroAnimCmd.TurnRight);
        }
        else
        {
            heroAnim.SetAnim(HeroAnim.HeroAnimCmd.StopHorizontal);
        }

        if (input.GetKey(InputKey.W))
        {
            attemptJump = true;
        }

        if (input.GetKey(InputKey.S))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * jumpStrength / 10, ForceMode2D.Impulse);
            heroAnim.SetAnim(HeroAnim.HeroAnimCmd.Falling);
        }

        input.ConsumeFrame();
    }
}
