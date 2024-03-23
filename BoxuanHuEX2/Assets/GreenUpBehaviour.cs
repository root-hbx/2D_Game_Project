using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenUpBehavior : MonoBehaviour
{
    static public int crash = 0;
    static public bool mFollowMousePosition = true;   // initialization of mouse-following
    public float mHeroSpeed = 20f;             // velocity of motion in units per second
    public float mHeroRotateSpeed = 90f / 2f;  // velocity of rotation (90-degrees in 2 seconds)
    static public int mTotalEggCount = 0;      // total number of eggs
    public float t1 = 0f;
    public float t2 = 0f;

    public float s = 0f;

    void Start()
    {
        crash = 0;
        //eggnum = 0;
        mFollowMousePosition = true;
        mHeroSpeed = 20f;
        mHeroRotateSpeed = 90f / 2f;
        mTotalEggCount = 0;
    }


    // Update is called once per frame
    void Update() 
    {
        Vector3 p = transform.localPosition;

        if (Input.GetKeyDown(KeyCode.M)) mFollowMousePosition = !mFollowMousePosition;

        if (mFollowMousePosition) {
            p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = 0f;
            // Vector3 mouseScreenPosition = Input.mousePosition;
            // mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
            // Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            // transform.position = mouseWorldPosition;
            transform.position = p;
            eggshot();
        }
        else {
            // if (Input.GetKey(KeyCode.W)) 
            //     p += ((mHeroSpeed * Time.smoothDeltaTime) * transform.up);
            // if (Input.GetKey(KeyCode.S)) 
            //     p -= ((mHeroSpeed * Time.smoothDeltaTime) * transform.up);
            // if (Input.GetKey(KeyCode.A)) 
            //     transform.Rotate(transform.forward,  mHeroRotateSpeed * Time.smoothDeltaTime);
            // if (Input.GetKey(KeyCode.D)) 
            //     transform.Rotate(transform.forward, -mHeroRotateSpeed * Time.smoothDeltaTime);
            if(Input.GetKey(KeyCode.D))
            {
                float angle = -45f * Time.deltaTime;
                transform.Rotate(new Vector3(0,0,1), angle);
            }
            else if(Input.GetKey(KeyCode.A))
            {
                float angle = 45f * Time.deltaTime;
                transform.Rotate(new Vector3(0, 0, 1), angle);
            }
            s = Input.GetAxis("Vertical") * 0.1f;
            mHeroSpeed += s;
            p = transform.localPosition;
            p += transform.up * mHeroSpeed * Time.deltaTime;
            transform.localPosition = p;
        }

            eggshot();
        //transform.localPosition = p;
        
    }

    public void eggshot() {
        t2 = Time.time;
        if (Input.GetKey(KeyCode.Space)) {
            if(t2 - t1 > 0.2) {
                GameObject e = Instantiate(Resources.Load("Prefab/Egg") as GameObject); // The Egg prefab is loaded from the Resources/Prefab
                e.transform.localPosition = transform.localPosition;
                e.transform.up = transform.up;
                // Debug.Log("Spawn Eggs:" + e.transform.localPosition);
                e.AddComponent<EggBehavior>(); // Add EggBehavior script to the newly instantiated Egg
                mTotalEggCount++;
                t1 = t2;
            }
        }
    }

    static public void OneLessEgg() { 
        mTotalEggCount--;  
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        /*
            - 在 Unity 中，当一个脚本中的方法被定义为 OnTriggerEnter2D，它必须是一个非静态方法，并且必须存在于一个继承自 MonoBehaviour 的类中。
            - 只有这样，Unity 引擎才能正确调用它，并在发生触发器碰撞时触发。
        */
        if (collision.name != "Egg(Clone)")
            crash++;
    }

    static public bool SeeFlag() {
        return mFollowMousePosition;
    }
    static public int CrashNum() {
        return crash;
    }
    static public void LessEgg() {
        mTotalEggCount--;
    }
    static public int GetEgg() {
        return mTotalEggCount;
    }
}