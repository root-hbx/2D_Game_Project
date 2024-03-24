using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class IterationSwitchUI : MonoBehaviour
{
    TMP_Text text;

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
        text = GetComponentInChildren<TMP_Text>();
        Assert.IsNotNull(text, "Text not found");
        text.gameObject.SetActive(false);
    }

    public void ShowContent(MessageType messageType)
    {
        text.gameObject.SetActive(true);
        switch (messageType)
        {
            case MessageType.Start:
                text.text = "Press Enter to start";
                break;
            case MessageType.Pass:
                text.text = "Passed! Press Enter to Continue";
                break;
            case MessageType.GameOver:
                text.text = "Failed! Press Enter to Restart";
                break;
            case MessageType.Undo:
                text.text = "Iter Undo. Press Enter to Continue";
                break;
            case MessageType.NextLevel:
                text.text = "Congratulation! All Iterations Completed.\n Press Enter to Watch Replay. Press B to go back to menu.";
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
}
