using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PortalBehaviour : MonoBehaviour
{

    public GameObject destPortal;

    void Start()
    {
        Assert.IsNotNull(destPortal, "Destination Portal is not set");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.transform.position = destPortal.transform.position;
        // disable collision between player and the other portal
        Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), destPortal.GetComponent<Collider2D>());
    }
}
