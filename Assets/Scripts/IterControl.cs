using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class IterControl : MonoBehaviour
{
    // Start is called before the first frame update
    StageManager Iter;

    public TMP_Text IterText;
    private int CurrentStage = 0;

    private int TotalStage = 0;
    void Start()
    {
        Iter = FindObjectOfType<StageManager>();
        Assert.IsNotNull(Iter, "StageManager not found");
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
