using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSwitchUI : MonoBehaviour
{
    TMP_Text text;

    void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        Assert.IsNotNull(text);
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
