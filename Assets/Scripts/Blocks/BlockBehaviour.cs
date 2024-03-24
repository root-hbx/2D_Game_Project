using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public enum BlockColor
    {
        Blue,
        Red
    }

    public enum BlockStatus
    {
        Transparent,
        Solid
    }

    public Sprite solidTexture;
    public Sprite transparentTexture;

    public BlockStatus oriStatus = BlockStatus.Solid;
    BlockStatus status = BlockStatus.Solid;
    public BlockColor color;

    void Start()
    {
        Reset();
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
        renderer.sprite = GetTexture(status);
        GetComponent<BoxCollider2D>().enabled = status == BlockStatus.Solid;
    }

    Sprite GetTexture(BlockStatus status)
    {
        return status == BlockStatus.Solid ? solidTexture : transparentTexture;
    }
}
