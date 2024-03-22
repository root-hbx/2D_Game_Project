using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordAction : MonoBehaviour
{
    static RecordAction instance;
    List<InputKey> recordActions = new();
    readonly ActualInput input = new();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputKey action = 0;

        foreach (var k in Enum.GetValues(typeof(InputKey)))
        {
            if (input.GetKey((InputKey)k))
            {
                action |= (InputKey)k;
            }
        }

        recordActions.Add(action);
    }

    public List<InputKey> TakeActions()
    {
        var actions = recordActions;
        recordActions = new List<InputKey>();
        return actions;
    }
}
