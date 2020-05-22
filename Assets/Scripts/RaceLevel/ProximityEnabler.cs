using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityEnabler : MonoBehaviour
{
    public float radius = 10;
    private SphereCollider sc;

    public void SetRadius(float radius)
    {
        sc = this.gameObject.GetComponent<SphereCollider>();
        sc.radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
