using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawn : MonoBehaviour
{
    public Vector3 initHeroPosition;

    void Start()
    {
        Vector3 heroPosition;
        if (GlobalState.instance != null)
        {
            heroPosition = GlobalState.instance.heroPosition;
        }
        else
        {
            heroPosition = initHeroPosition;
        }
        Instantiate(Resources.Load("Prefabs/Hero"), heroPosition, Quaternion.identity);
    }
}
