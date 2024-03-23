using UnityEditor;
using UnityEngine;

public class PlaneBehavior : MonoBehaviour
{
    public bool mFollowMousePosition = true;
    public float mHeroSpeed = 5f;
    public float mHeroRotateSpeed = 90f / 2f; // 90-degrees in 2 seconds

    // public float speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        mHeroSpeed = 100;
        // Rotate the object in Z-axis by a random degree between -45 and +45
        float randomRotation = Random.Range(-45f, 45f);
        transform.Rotate(0f, 0f, randomRotation);

        // Compute a random speed between 0.5 and 1 units/sec
        // speed = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.localPosition;

        if (Input.GetKeyDown(KeyCode.Space))
            mFollowMousePosition = !mFollowMousePosition;

        if (mFollowMousePosition)
        {
            p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = 0f;  // <-- this is VERY IMPORTANT!
            // Debug.Log("Screen Point:" + Input.mousePosition + "  World Point:" + p);
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
                p += ((mHeroSpeed * Time.smoothDeltaTime) * transform.up);

            if (Input.GetKey(KeyCode.S))
                p -= ((mHeroSpeed * Time.smoothDeltaTime) * transform.up);

            if (Input.GetKey(KeyCode.A))
                transform.Rotate(transform.forward, mHeroRotateSpeed * Time.smoothDeltaTime);

            if (Input.GetKey(KeyCode.D))
                transform.Rotate(transform.forward, -mHeroRotateSpeed * Time.smoothDeltaTime);
        }

        // Move the object forward at the computed speed
        transform.localPosition += transform.up * mHeroSpeed * Time.deltaTime;
    }
}
