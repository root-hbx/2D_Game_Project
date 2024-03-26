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
        if (input.GetKey(InputKey.D))
        {
            direction = Direction.Right;
        }
        else if (input.GetKey(InputKey.A))
        {
            direction = Direction.Left;
        }

        if (input.GetKey(InputKey.Attack))
        {
            if (Time.time - lastShootTime > kShootInterval)
            {
                lastShootTime = Time.time;
                Shoot();
            }
        }
        input.ConsumeFrame();
    }

    void Shoot()
    {
        Vector3 bulletPosition = transform.position;
        Bounds bounds = GetComponent<Renderer>().bounds;
        bulletPosition.x += (direction == Direction.Right ? 1 : -1) * bounds.size.x;
        bulletPosition.y += bounds.size.y / 2;
        float rotateAngle = direction == Direction.Right ? -90 : 90;
        Instantiate(Resources.Load("Prefabs/Bullet"), bulletPosition, Quaternion.Euler(0, 0, rotateAngle));
    }
}
