using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SpeedrunRecorder : MonoBehaviour
{
    TMP_Text text;
    bool completed = false;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = "Total Time: 0:00:000";
    }

    // Update is called once per frame
    void Update()
    {
        if (completed)
        {
            return;
        }

        var time = Time.timeSinceLevelLoad;
        var minutes = (int)time / 60;
        var seconds = (int)time % 60;
        var milliseconds = (int)(time * 1000) % 1000;
        text.text = $"Total Time: {minutes}:{seconds:D2}:{milliseconds:D3}";
    }

    public void Complete()
    {
        if (completed)
        {
            return;
        }

        completed = true;
        if (SpeedrunManager.instance != null)
        {
            SpeedrunManager.instance.Record((int)(Time.timeSinceLevelLoad * 1000));
        }
    }
}
