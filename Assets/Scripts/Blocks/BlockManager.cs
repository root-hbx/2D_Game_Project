using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public SwitchBehaviour[] allSwitches;
    public BlockBehaviour[] redBlocks,blueBlocks;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void BlocksChange(int color)
    {
        if(color == 1)
        {
            for(int i = 0; i< redBlocks.Length; ++i)
            {
                redBlocks[i].ChangeStatus();
            }
        }
        else
        {
            for (int i = 0; i < blueBlocks.Length; ++i)
            {
                blueBlocks[i].ChangeStatus();
            }
        }
        
    }
    public void Reset()
    {
        for (int i = 0; i < redBlocks.Length; ++i)
        {
            redBlocks[i].Reset();
        }
        for (int i = 0; i < blueBlocks.Length; ++i)
        {
            blueBlocks[i].Reset();
        }
        for (int i = 0; i < allSwitches.Length; ++i)
        {
            allSwitches[i].Reset();
        }
    }
}
