using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    public IInput input = new ActualInput();

    readonly float moveSpeed = 80.0f;
    readonly float jumpStrength = 30.0f;

    bool attemptJump = false;
    bool isJumping = false;

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
            }
            attemptJump = false;
        }
    }

    void UpdateMovement()
    {
        if (input.GetKey(InputKey.A))
        {
            transform.Translate(moveSpeed * Time.smoothDeltaTime * Vector3.left);
        }
        else if (input.GetKey(InputKey.D))
        {
            transform.Translate(moveSpeed * Time.smoothDeltaTime * Vector3.right);
        }

        if (input.GetKey(InputKey.W))
        {
            attemptJump = true;
        }

        if (input.GetKey(InputKey.S))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * jumpStrength / 10, ForceMode2D.Impulse);
        }

        input.ConsumeFrame();
    }
}
