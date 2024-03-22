using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    enum Direction
    {
        Left,
        Right
    }
    Direction direction = Direction.Right;

    public IInput input = new ActualInput();

    float lastShootTime = 0;
    readonly float shootInterval = 0.3f;

    // Update is called once per frame
    void Update()
    {
        if (input.GetKey(InputKey.D))
        {
            direction = Direction.Right;
        }
        else if (input.GetKey(InputKey.A))
        {
            direction = Direction.Left;
        }

        if (input.GetKey(InputKey.Space))
        {
            if (Time.time - lastShootTime > shootInterval)
            {
                lastShootTime = Time.time;
                Shoot();
            }
        }
        input.ConsumeFrame();
    }

    void Shoot()
    {
        // the bullet should outside the player
        Vector3 bulletPosition = transform.position;
        bulletPosition.x += (direction == Direction.Right ? 1 : -1) * 10;
        float rotateAngle = direction == Direction.Right ? -90 : 90;
        Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, bulletPosition, Quaternion.Euler(0, 0, rotateAngle));
    }
}
