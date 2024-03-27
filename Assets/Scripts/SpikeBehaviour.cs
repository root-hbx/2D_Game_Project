using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<MoveBehaviour>().Die();
            Destroy(other.gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Hero"))
        {
            other.gameObject.GetComponent<MoveBehaviour>().Die();
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
}
