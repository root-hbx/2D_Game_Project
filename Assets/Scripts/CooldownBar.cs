using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    Image timeImage;
    StageManager stageManager;

    public float time;
    bool isCooldown;


    void Start()
    {
        stageManager = GetComponent<StageManager>();
        timeImage = GameObject.Find("Cooldown/CooldownBar/CooldownProgress").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            timeImage.fillAmount += 1 / time * Time.deltaTime;
            if (timeImage.fillAmount >= 1)
            {
                stageManager.GameOver();
                isCooldown = false;
            }
        }
    }

    public void Reset()
    {
        timeImage.fillAmount = 0;
        isCooldown = false;
    }

    public void StartCooldown()
    {
        isCooldown = true;
    }
}
