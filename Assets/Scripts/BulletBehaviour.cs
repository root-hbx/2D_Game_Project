using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    readonly float moveSpeed = 100.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.smoothDeltaTime * Vector3.up);

        // destroy the egg if it goes off screen
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
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
