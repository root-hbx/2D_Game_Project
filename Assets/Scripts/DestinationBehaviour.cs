using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationBehaviour : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Hero"))
        {
            var stageManager = FindObjectOfType<StageManager>();
            if (stageManager.IsHeroStage)
            {
                stageManager.NextStage();
            }
            else
            {
                stageManager.GameOver();
            }
        }
    }
}
