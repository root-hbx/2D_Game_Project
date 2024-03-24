using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DestinationBehaviour : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Assert.IsNotNull(animator, "Animator not found");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Hero"))
        {
            animator.SetTrigger("Pressed");
            var iterationManager = FindObjectOfType<IterationManager>();
            if (iterationManager.IsHeroIteration)
            {
                iterationManager.NextIteration();
            }
            else
            {
                iterationManager.GameOver();
            }
        }
    }
}
