using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private string color;
#pragma warning restore 0649
    private ScoreController sc;

    private void Awake()
    {
        sc = GameObject.Find("ScoreController").GetComponent<ScoreController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                sc.OnCrystalCollect(color);
                TriggerCrystal();
                break;
            case "Spell":
                sc.OnCrystalCollect(color);
                TriggerCrystal();
                break;
        }
    }

    private void TriggerCrystal()
    {
        this.gameObject.SetActive(false);
    }
}
