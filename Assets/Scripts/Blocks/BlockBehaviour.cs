using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite [] curTexture;
    public int oriStatus = 0;
    private int status = 0;
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
        renderer.sprite = curTexture[status=oriStatus];
        GetComponent<BoxCollider2D>().enabled = (oriStatus == 1);
    }
    public void ChangeStatus()
    {
        status ^= 1;
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = curTexture[status];
        GetComponent<BoxCollider2D>().enabled = (status==1);
    }
}
