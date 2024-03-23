using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSwitchUI : MonoBehaviour
{
    static StageSwitchUI instance;

    TMP_Text text = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            text = GetComponentInChildren<TMP_Text>(true);
            Debug.Assert(text != null, "Text not found");
        }
        else
        {
            Destroy(gameObject);
        }
        instance.Disable();
    }

    // Start is called before the first frame update
    void Disable()
    {
        text.gameObject.SetActive(false);
    }

    public void ShowStartIndicator(bool show)
    {
        Debug.Log("ShowStartIndicator" + show);
        text.gameObject.SetActive(show);
        text.text = "Press Enter to Start";
    }

    public void GameOver()
    {
        text.gameObject.SetActive(true);
        text.text = "Game Over";
    }

    public void NextLevel()
    {
        text.gameObject.SetActive(true);
        text.text = "Congratulation! All Stages Completed";
    }
}
