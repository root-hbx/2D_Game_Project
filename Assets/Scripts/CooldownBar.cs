using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public float time;
    public Image imagecooldown;
    public bool isCooldowm = false;//开始计时
    public bool over = false;//游戏结束!!!!!!!!!!!!!!
    private StageManager pl;
    // Start is called before the first frame update
    void Start()
    {
        pl = GameObject.Find("Global State").GetComponent<StageManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(isCooldowm)
        {
            imagecooldown.fillAmount +=1 / time * Time.deltaTime;
            if(imagecooldown.fillAmount >= 1)
            {
                pl.GameOver();
                isCooldowm = false;
            }
        }
        if(over)
        {
            imagecooldown.fillAmount = 0;
            isCooldowm = false;
            over = false;
        }
    }
    
}
