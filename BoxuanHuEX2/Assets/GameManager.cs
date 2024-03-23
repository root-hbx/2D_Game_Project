using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GreenUpBehavior myarrow = null;
    public Text mytext = null;
    public PlaneBehavior myplane = null;
    public bool flag;
    public string a,c,e,g;
    public int b,d,f;
    // Start is called before the first frame update
    void Start()
    {
        EggBehavior.SetGreenArrow(myarrow);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("quit");
            Application.Quit();

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        flag = GreenUpBehavior.SeeFlag();
        
        if (flag) a = "mouse";
        else a = "keyboard";

        b = GreenUpBehavior.CrashNum();
        c = b.ToString();
        d = GreenUpBehavior.GetEgg();
        e = d.ToString();
        f = myplane.GetPlane();
        g = f.ToString();
        
        Debug.Log("sb");
        string abs = "HERO: drive(" + a + ") " + "TouchedEnemy(" + c + ")      ";
        abs += "EGG: OnScreen( " + e + ")     ";
        abs += "ENEMY: Count(10) Destroyed(" + g + ")";
        mytext.text = abs;

        //myarrow.tryyy();
    }
}
