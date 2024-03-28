using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    SwitchBehaviour[] allSwitches;
    BlockBehaviour[] blocks;

    void Start()
    {
        Reset();
    }

    public void BlocksChange(BlockBehaviour.BlockColor color)
    {
        foreach (var block in blocks)
        {
            if (block.color == color)
            {
                block.ChangeStatus();
            }
        }
    }

    public void Reset()
    {
        blocks = FindObjectsOfType<BlockBehaviour>();
        foreach (var block in blocks)
        {
            block.Reset();
        }
        allSwitches = FindObjectsOfType<SwitchBehaviour>();
        foreach (var switchBlock in allSwitches)
        {
            switchBlock.Reset();
        }
    }
}
