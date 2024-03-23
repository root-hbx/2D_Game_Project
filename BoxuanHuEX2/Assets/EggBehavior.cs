using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    static private GreenUpBehavior sGreenArrow = null;
    static public void SetGreenArrow(GreenUpBehavior g) { 
        sGreenArrow = g; 
    }

    private const float kEggSpeed = 40f;
    private const int kLifeTime = 30000; // The lifetime of the egg is 300 frames
    private int mLifeCount = 0; 

    void Start() {
        mLifeCount = kLifeTime; 
    }

    void Update() {
        transform.position += transform.up * (kEggSpeed * Time.smoothDeltaTime);
        mLifeCount--;
        // if (mLifeCount <= 0)
        // {
        //     Destroy(transform.gameObject);  // it will kill itself
        //     sGreenArrow.OneLessEgg();
        // }
        
        if(transform.position.x > 100 || transform.position.x < -100 || transform.position.y > 100 || transform.position.y < -100) {
            Destroy(transform.gameObject);  // it will kill itself
            GreenUpBehavior.OneLessEgg();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("1234566");
        if(collision.gameObject.name!="GreenUp")
        {
            Destroy(gameObject);
            GreenUpBehavior.OneLessEgg();
        }
    }

}
