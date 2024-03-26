using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : IManualBehaviour
{
    enum Direction
    {
        Left,
        Right
    }
    Direction direction = Direction.Right;
    public IInput input = new ActualInput();

    float lastShootTime = 0;
    const float kShootInterval = 0.3f;

    public override void ManualUpdate()
    {
        direction = transform.localScale.x < 0 ? Direction.Right : Direction.Left;
        if (input.GetKey(InputKey.Attack))
        {
            if (Time.time - lastShootTime > kShootInterval)
            {
                lastShootTime = Time.time;
                Shoot();
                GetComponent<Animator>().ResetTrigger("Attack");
                GetComponent<Animator>().SetTrigger("Attack");
            }
        }
        input.ConsumeFrame();
    }

    void Shoot()
    {
        StartCoroutine(GenerateBullet());
    }
    IEnumerator GenerateBullet()
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 bulletPosition = transform.position;
        Collider2D collider = GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        bulletPosition.x += direction == Direction.Right ? colliderSize.x / 2 + 0.8f : -colliderSize.x / 2 - 0.8f;
        bulletPosition.y += colliderSize.y / 2;
        float rotateAngle = direction == Direction.Right ? -90 : 90;
        Instantiate(Resources.Load("Prefabs/Bullet"), bulletPosition, Quaternion.Euler(0, 0, rotateAngle));
    }
}
