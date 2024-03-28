using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class IterationSwitchUI : MonoBehaviour
{
    TMP_Text hintText;
    TMP_Text levelNameText;
    TMP_Text passOrFailText;
    TMP_Text goalText;

    public enum MessageType
    {
        Start,
        Pass,
        GameOver,
        Undo,
        NextLevel
    }
    public enum GoalType
    {
        ReachDestination,
        KillHero,
    }

    void Awake()
    {
        var texts = GetComponentsInChildren<TMP_Text>();
        Assert.IsNotNull(texts, "Text not found");
        Assert.IsTrue(texts.Length >= 4, "Texts not set correctly");
        hintText = texts[0];
        levelNameText = texts[1];
        passOrFailText = texts[2];
        goalText = texts[3];
        hintText.gameObject.SetActive(false);
    }

    public void ShowContent(MessageType messageType)
    {
        switch (messageType)
        {
            case MessageType.Start:
                if (passOrFailText.text != "Iter Undo")
                    passOrFailText.gameObject.SetActive(false);
                hintText.gameObject.SetActive(true);
                hintText.text = "Press Any Key to start";
                break;
            case MessageType.Pass:
                passOrFailText.gameObject.SetActive(true);
                passOrFailText.text = "Passed";
                passOrFailText.color = Color.green;
                break;
            case MessageType.GameOver:
                passOrFailText.gameObject.SetActive(true);
                passOrFailText.text = "Failed";
                passOrFailText.color = Color.red;
                break;
            case MessageType.Undo:
                passOrFailText.gameObject.SetActive(true);
                passOrFailText.text = "Iter Undo";
                passOrFailText.color = Color.yellow;
                StartCoroutine(StopShowContentLerp(passOrFailText));
                break;
            case MessageType.NextLevel:
                hintText.gameObject.SetActive(true);
                hintText.text = "Press Any Key to Watch Replay.\n Press B to go back to menu.";
                passOrFailText.gameObject.SetActive(true);
                passOrFailText.text = "Congratulation";
                passOrFailText.color = Color.green;
                break;
        }
    }

    public void ShowGoal(GoalType goalType)
    {
        switch (goalType)
        {
            case GoalType.ReachDestination:
                goalText.text = "Goal: Reach Cup";
                break;
            case GoalType.KillHero:
                goalText.text = "Goal: Kill Alice";
                break;
        }
    }

    public void ShowContent(string message)
    {
        hintText.gameObject.SetActive(true);
        hintText.text = message;
    }

    public void StopShowContent()
    {
        hintText.gameObject.SetActive(false);
    }

    IEnumerator StopShowContentLerp(TMP_Text text)
    {
        float fadeDuration = 1f; // Duration of the fade in seconds
        float elapsedTime = 0f;
        Color startColor = text.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Fade out to transparent

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            text.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

        text.gameObject.SetActive(false);
    }

    public void ShowLevelName(string levelName)
    {
        levelNameText.gameObject.SetActive(true);
        levelNameText.text = levelName;
        StartCoroutine(StopShowContentLerp(levelNameText));
    }
}
