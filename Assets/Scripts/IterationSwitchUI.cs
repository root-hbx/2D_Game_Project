using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class IterationSwitchUI : MonoBehaviour
{
    TMP_Text text;
    TMP_Text levelNameText;

    public enum MessageType
    {
        Start,
        Pass,
        GameOver,
        Undo,
        NextLevel
    }

    void Awake()
    {
        var texts = GetComponentsInChildren<TMP_Text>();
        Assert.IsNotNull(texts, "Text not found");
        Assert.IsTrue(texts.Length == 2, "Texts not set correctly");
        text = texts[0];
        levelNameText = texts[1];
        text.gameObject.SetActive(false);
    }

    public void ShowContent(MessageType messageType)
    {
        text.gameObject.SetActive(true);
        switch (messageType)
        {
            case MessageType.Start:
                text.text = "Press Any Key to start";
                break;
            case MessageType.Pass:
                text.text = "Passed! Press Any Key to Continue";
                break;
            case MessageType.GameOver:
                text.text = "Failed! Press Any Key to Restart";
                break;
            case MessageType.Undo:
                text.text = "Iter Undo. Press Any Key to Continue";
                break;
            case MessageType.NextLevel:
                text.text = "Congratulation! All Iterations Completed.\n Press Any Key to Watch Replay. Press B to go back to menu.";
                break;
        }
    }

    public void ShowContent(string message)
    {
        text.gameObject.SetActive(true);
        text.text = message;
    }

    public void StopShowContent()
    {
        text.gameObject.SetActive(false);
    }

    IEnumerator StopShowLevelName()
    {
        float fadeDuration = 1f; // Duration of the fade in seconds
        float elapsedTime = 0f;
        Color startColor = levelNameText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Fade out to transparent

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            levelNameText.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

        levelNameText.gameObject.SetActive(false);
    }

    public void ShowLevelName(string levelName)
    {
        levelNameText.gameObject.SetActive(true);
        levelNameText.text = levelName;
        StartCoroutine(StopShowLevelName());
    }
}
