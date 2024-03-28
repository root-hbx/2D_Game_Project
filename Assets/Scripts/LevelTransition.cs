using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string nextSceneName = "";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            var hero = GameObject.FindGameObjectWithTag("Hero");
            if (GetComponent<Renderer>().bounds.Contains(hero.transform.position))
            {
                LoadNextScene();
            }
        }
    }

    void LoadNextScene()
    {
        GlobalState.instance.heroPosition = GameObject.FindGameObjectWithTag("Hero").transform.position;
        var matched = int.TryParse(Regex.Match(nextSceneName, @"\d+$").Value, out int level);
        if (matched)
        {
            SpeedrunManager.instance.level = level;
        }

        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
