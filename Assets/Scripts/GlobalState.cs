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
        if (Input.GetKeyDown(KeyCode.B) && SceneManager.GetActiveScene().name != "Directory")
        {
            SceneManager.LoadSceneAsync("Directory");
        }
    }
}
