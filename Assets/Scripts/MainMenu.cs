using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Directory");
        }
    }
}
