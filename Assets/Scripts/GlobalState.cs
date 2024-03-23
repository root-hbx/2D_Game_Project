using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1920, 1200, true);
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1f / 60f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
