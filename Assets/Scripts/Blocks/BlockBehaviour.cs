using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public enum BlockColor
    {
        Blue = 0,
        Red = 1
    }
    public enum BlockStatus
    {
        Transparent = 0,
        Solid = 1
    }
    // Start is called before the first frame update
    public Sprite solidTexture;
    public Sprite transparentTexture;

    public BlockStatus oriStatus = BlockStatus.Solid;
    private BlockStatus status = BlockStatus.Solid;
    public BlockColor color;
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Reset()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = GetTexture(oriStatus);
        GetComponent<BoxCollider2D>().enabled = oriStatus == BlockStatus.Solid;
        status = oriStatus;
    }
    public void ChangeStatus()
    {
        // toggle the status
        status = status == BlockStatus.Solid ? BlockStatus.Transparent : BlockStatus.Solid;

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        Debug.Log(status);
        renderer.sprite = GetTexture(status);
        GetComponent<BoxCollider2D>().enabled = status == BlockStatus.Solid;
    }

    Sprite GetTexture(BlockStatus status)
    {
        return status == BlockStatus.Solid ? solidTexture : transparentTexture;
    }
}
