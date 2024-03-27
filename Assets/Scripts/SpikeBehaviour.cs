using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehaviour : MonoBehaviour
{
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
