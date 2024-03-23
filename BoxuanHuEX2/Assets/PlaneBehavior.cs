using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEditor.PackageManager;
using UnityEngine;

public class PlaneBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 pos;

    
    static public int numbee = 0;
    private int Count = 0;

    void Start() 
    {
        Debug.Log("Beginning");
        pos.x = Random.Range(-90, 90);
        pos.y = Random.Range(-90, 90);
        transform.localPosition = pos;
    }

    // Update is called once per frame
    void Update() 
    {
        pos.x = Random.Range(-90, 90);
        pos.y = Random.Range(-90, 90);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Debug.Log("Crash is happening!");
        if (collision.gameObject.name == "GreenUp") {
            DestroyAndRespawn();
        }
        else {
            Count++;
            if (Count >= 4) {
                DestroyAndRespawn();
            }
            else {
                ChangeTransparency(0.8f);
            }
        }
    }

    private void ChangeTransparency(float alphaFactor)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) {
            Color color = renderer.material.color;
            color.a *= alphaFactor;
            renderer.material.color = color;
        }
    }

    private void DestroyAndRespawn()
    {
        numbee++;
        
        Destroy(gameObject);

        Vector3 newPosition = new Vector3(Random.Range(-90f, 90f), Random.Range(-90f, 90f), transform.position.z);

        GameObject e =  Instantiate(Resources.Load("Prefab/Plane") as GameObject);
        
        e.transform.localPosition = newPosition;
    }
    public int GetPlane()
    {
        return numbee;
    }
}
