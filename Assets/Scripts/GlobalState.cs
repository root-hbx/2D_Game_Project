using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalState : MonoBehaviour
{
    public static GlobalState instance;

    [HideInInspector]
    public Vector3 heroPosition;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void SetConfig()
    {
        Screen.SetResolution(1920, 1200, false);
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1f / 60f;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadSceneAsync("Directory");
        }
    }
}
