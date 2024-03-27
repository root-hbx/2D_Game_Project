using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBlockBehaviour : IManualBehaviour
{
    Rigidbody2D rb;

    Vector3 startPos;
    bool isReturning;
    float returnTime;
    const float kReturnDelay = 1f;

    bool IsGrounded => Physics2D.OverlapCircle((Vector2)transform.position + Vector2.down * 2, 0.25f, 1 << LayerMask.NameToLayer("Ground"));

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    public override void ManualUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 20f, 1 << LayerMask.NameToLayer("Hero") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Ground"));
        if (hit.collider != null && (hit.collider.CompareTag("Hero") || hit.collider.CompareTag("Enemy")))
        {
            rb.gravityScale = 10f;
            isReturning = false;
        }

        if (IsGrounded && !isReturning)
        {
            isReturning = true;
            rb.gravityScale = 0;
            returnTime = Time.time;
        }

        if (isReturning && Time.time - returnTime > kReturnDelay)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, 0.1f);
            if (transform.position == startPos)
            {
                isReturning = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Kill(other);
            return;
        }

        if (other.gameObject.CompareTag("Hero"))
        {
            Kill(other);
            var iterationManager = FindObjectOfType<IterationManager>();
            if (iterationManager.IsHeroIteration)
            {
                iterationManager.GameOver();
            }
            else
            {
                iterationManager.NextIteration();
            }
        }
    }

    public void Reset()
    {
        transform.position = startPos;
        isReturning = false;
        rb.gravityScale = 0;
    }
    static void Kill(Collision2D other)
    {
        if (other.gameObject.GetComponent<MoveBehaviour>().enabled)
        {
            other.gameObject.GetComponent<MoveBehaviour>().Die();
            other.gameObject.GetComponent<ShootBehaviour>().enabled = false;
            other.gameObject.GetComponent<MoveBehaviour>().enabled = false;
        }
    }
}
