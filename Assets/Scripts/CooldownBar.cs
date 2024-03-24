using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public float time;
    private Image CooldownImage = null;
    public bool isCooldown = false;//开始计时
    public bool over = false;//游戏结束!!!!!!!!!!!!!!
    private StageManager pl;
    // Start is called before the first frame update
    void Start()
    {
        pl = GetComponent<StageManager>();
        CooldownImage = GameObject.Find("Cooldown/CooldownBar/CooldownProgress").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isCooldown)
        {
            CooldownImage.fillAmount += 1 / time * Time.deltaTime;
            if (CooldownImage.fillAmount >= 1)
            {
                pl.GameOver();
                isCooldown = false;
            }
        }
        if (over)
        {
            CooldownImage.fillAmount = 0;
            isCooldown = false;
            over = false;
        }
    }

}
