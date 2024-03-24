using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void SetConfig()
    {
        Screen.SetResolution(1920, 1200, true);
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1f / 60f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }
}
