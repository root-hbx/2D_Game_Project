using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string nextSceneName = "";        // define the next scene name

    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            var heroes = GameObject.FindGameObjectsWithTag("Hero");
            foreach (var hero in heroes)
            {
                if (GetComponent<Renderer>().bounds.Contains(hero.transform.position))
                {
                    LoadNextScene();
                    break;
                }
            }
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
