using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    Image timeImage;
    IterationManager iterationManager;

    public float limitTime;
    bool running;


    void Start()
    {
        Assert.IsTrue(limitTime > 0, "Limit time must be greater than 0");
        iterationManager = GetComponent<IterationManager>();
        Assert.IsNotNull(iterationManager, "IterationManager not found");
        timeImage = GameObject.Find("TimeLimitBar/Time Limit Progress").GetComponent<Image>();
        Assert.IsNotNull(timeImage, "Image not found");
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            timeImage.fillAmount -= 1 / limitTime * Time.deltaTime;
            if (timeImage.fillAmount <= 0)
            {
                iterationManager.GameOver();
                running = false;
            }
        }
    }

    public void Reset()
    {
        timeImage.fillAmount = 1;
        running = false;
    }

    public void Run()
    {
        running = true;
    }
}
