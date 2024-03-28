using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    public BlockBehaviour.BlockColor oriColor = BlockBehaviour.BlockColor.Blue;

    void Start()
    {
        if (oriColor == BlockBehaviour.BlockColor.Blue)
        {
            GetComponent<SpriteRenderer>().sprite=Resources.Load<Sprite>("Textures/Blocks/blue_c");
        }
        else {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Blocks/red_c");
        }
        Reset();
    }

    public void Reset()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            var blockManager = FindObjectOfType<BlockManager>();
            blockManager.BlocksChange(oriColor);
        }
    }
}
