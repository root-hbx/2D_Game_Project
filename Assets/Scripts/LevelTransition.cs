using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public GameObject doorToActivate = null; // define the door to activate in this level
    public GameObject prefabToMatch = null;  // need to match the door position at least
    void Start() 
    {   // You have to update this part by each single level
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && IsPrefabMatchingPosition())
        {   // the gamer press <Enter>  AND the door is at the same position as the prefab
                LoadNextScene();
        }
    }
    
    bool IsPrefabMatchingPosition()
    {
        if (doorToActivate != null && prefabToMatch != null)
        {
            return doorToActivate.transform.position == prefabToMatch.transform.position;
        }
        return false;
    }

    void LoadNextScene()
    {   // This part needs to be updated by each single level 
        SceneManager.LoadScene("SceneNumber_Level_n"); 
    }
}
