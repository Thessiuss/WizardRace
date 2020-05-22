using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFrontTriggerHandler : MonoBehaviour
{
    private LevelController lc;
    private void Awake()
    {
        lc = GameObject.Find("LevelController").GetComponent<LevelController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "RearTrigger")
        {
            lc.RemoveRearTerrain(this.gameObject);
        }
    }
}
