using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSwitchUI : MonoBehaviour
{
    static StageSwitchUI instance;

    GameObject panel;
    TMP_Text text;
    Button button;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            panel = GetComponentInChildren<Image>().gameObject;
            text = GetComponentInChildren<TMP_Text>();
            button = GetComponentInChildren<Button>();
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
        panel.SetActive(false);
        text.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        button.onClick.AddListener(OnClick);
    }

    public void NextStage(int stage)
    {
        panel.SetActive(true);
        text.gameObject.SetActive(true);
        text.text = $"Congratulation! Stage {stage} Completed";
        button.gameObject.SetActive(true);
        button.GetComponentInChildren<TMP_Text>().text = "Next Stage";
    }

    public void GameOver()
    {
        panel.SetActive(true);
        text.gameObject.SetActive(true);
        text.text = "Game Over";
        button.gameObject.SetActive(true);
        button.GetComponentInChildren<TMP_Text>().text = "Restart";
    }

    public void NextLevel()
    {
        panel.SetActive(true);
        text.gameObject.SetActive(true);
        text.text = "Congratulation! All Stages Completed";
    }

    void OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
