using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class IterationText : MonoBehaviour
{
    // Start is called before the first frame update
    IterationManager iterationManager;
    TMP_Text text;

    void Start()
    {
        iterationManager = FindObjectOfType<IterationManager>();
        Assert.IsNotNull(iterationManager, "IterationManager not found");
        text = GetComponentInChildren<TMP_Text>();
        Assert.IsNotNull(text, "TextMeshPro not found");
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Iteration: {iterationManager.currentIteration} / {iterationManager.iterations}";
    }
}
