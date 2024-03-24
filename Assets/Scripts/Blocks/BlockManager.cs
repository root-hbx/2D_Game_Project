using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private SwitchBehaviour[] allSwitches;
    private BlockBehaviour[] blocks;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void BlocksChange(BlockBehaviour.BlockColor color)
    {
        foreach (var block in blocks)
        {
            if (block.color == color)
            {
                Debug.Log("Change block color");
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
