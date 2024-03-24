using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    public BlockBehaviour.BlockColor oriColor = BlockBehaviour.BlockColor.Blue;
    // Start is called before the first frame update
    void Start()
    {
        if (oriColor == 0)
        {
            GetComponent<SpriteRenderer>().sprite=Resources.Load<Sprite>("Textures/Blocks/blue_c");
        }
        else {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Blocks/red_c");
        }
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            Debug.Log("Switch hit");
            GetComponent<Renderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            var blockManager = FindObjectOfType<BlockManager>();
            blockManager.BlocksChange(oriColor);
        }
    }
}
