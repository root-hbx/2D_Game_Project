using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IterControl : MonoBehaviour
{
    // Start is called before the first frame update
    StageManager Iter = null;

    public TMP_Text IterText =  null;
    private int CurrentStage = 0;

    private int TotalStage = 0;
    void Start()
    {
        Iter = GameObject.FindObjectOfType<StageManager>();
        TotalStage = Iter.stages;
        CurrentStage = Iter.currentStage;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentStage = Iter.currentStage;
        IterText.text = "Stage: " + CurrentStage + "/" + TotalStage;
    }
}
