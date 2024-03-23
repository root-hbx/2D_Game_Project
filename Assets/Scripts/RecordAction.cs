using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordAction : MonoBehaviour
{
    List<InputKey> recordActions = new();
    readonly ActualInput input = new();

    // Update is called once per frame
    void FixedUpdate()
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
