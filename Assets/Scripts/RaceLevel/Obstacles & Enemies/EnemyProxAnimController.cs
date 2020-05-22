using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProxAnimController : MonoBehaviour {
    
#pragma warning disable 0649
    [SerializeField] private Animator anim;
    [SerializeField] private List<GameObject> enableObjects;
#pragma warning restore 0649
    private bool playerEnter;
    // Use this for initialization

    public bool GetPlayerEnter()
    {
        return playerEnter;
    }
    private void OnEnable()
    {
        playerEnter = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && (anim != null))
        {
            playerEnter = true;
            HandleAnimation();
            EnableObjects();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player") && (anim != null))
        {
            playerEnter = false;
            HandleAnimation();
            DisableObjects();
        }
    }

    private void HandleAnimation()
    {
        anim.SetBool("Attack", playerEnter);
    }

    private void DisableObjects()
    {
        if (enableObjects.Count > 0)
        {
            for (int i = 0; i < enableObjects.Count; i++)
            {
                enableObjects[i].SetActive(false);
            }
        }
    }

    private void EnableObjects()
    {
        if (enableObjects.Count > 0)
        {
            for (int i = 0; i < enableObjects.Count; i++)
            {
                enableObjects[i].SetActive(true);
            }
        }
    }
}
