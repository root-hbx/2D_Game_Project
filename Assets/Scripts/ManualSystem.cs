using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class IManualBehaviour : MonoBehaviour, IComparable
{
    abstract public void ManualUpdate();

    public void Awake()
    {
        ManualSystem.instance.Add(this);
    }

    public void OnDestroy()
    {
        ManualSystem.instance.Remove(this);
    }

    public int CompareTo(object obj)
    {
        return GetHashCode().CompareTo(obj.GetHashCode());
    }
}

public class ManualSystem : MonoBehaviour
{
    public static ManualSystem instance;
    SortedSet<IManualBehaviour> scripts = new();
    List<IManualBehaviour> scriptsToAdd = new();
    readonly object lockScriptsToAdd = new();
    List<IManualBehaviour> scriptsToRemove = new();
    readonly object lockScriptsToRemove = new();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        lock (lockScriptsToAdd)
        {
            foreach (var script in scriptsToAdd)
            {
                scripts.Add(script);
            }
            scriptsToAdd.Clear();
        }

        lock (scriptsToRemove)
        {
            foreach (var script in scriptsToRemove)
            {
                scripts.Remove(script);
            }
            scriptsToRemove.Clear();
        }

        foreach (var script in scripts)
        {
            if (script.enabled)
            {
                script.ManualUpdate();
            }
        }

        if (Physics2D.simulationMode != SimulationMode2D.Script)
            return;

        Physics2D.Simulate(Time.fixedDeltaTime);
    }

    public void Add(IManualBehaviour script)
    {
        lock (lockScriptsToAdd)
        {
            scriptsToAdd.Add(script);
        }
    }

    public void Remove(IManualBehaviour script)
    {
        lock (lockScriptsToRemove)
        {
            scriptsToRemove.Add(script);
        }
    }
}
