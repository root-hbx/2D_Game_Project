using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    const float kMoveSpeed = 20.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(kMoveSpeed * Time.smoothDeltaTime * Vector3.up);

        // destroy the egg if it goes off screen
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet")) return;
        if (other.gameObject.CompareTag("Portal")) return;

        Destroy(gameObject);

        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Hero"))
        {
            var stageManager = FindObjectOfType<StageManager>();
            if (stageManager.IsHeroStage)
            {
                stageManager.GameOver();
            }
            else
            {
                stageManager.NextStage();
            }
        }
    }
}
