using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainRearTriggerHandler : MonoBehaviour
{
    private LevelController lc;
    private void Awake()
    {
        lc = GameObject.Find("LevelController").GetComponent<LevelController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "FrontTrigger")
        {
            lc.RemoveFrontTerrain(this.gameObject);
        }
    }
}
