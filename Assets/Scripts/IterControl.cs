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
    int CurrentStage => Iter.currentStage;
    int TotalStage => Iter.stages;

    void Start()
    {
        Iter = FindObjectOfType<StageManager>();
        Assert.IsNotNull(Iter, "StageManager not found");
    }

    // Update is called once per frame
    void Update()
    {
        IterText.text = "Stage: " + CurrentStage + "/" + TotalStage;
    }
}
