using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterActivator : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Proximity")
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
