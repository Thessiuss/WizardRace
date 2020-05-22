using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollAnimController : MonoBehaviour
{
    // Every Animator controller needs an Attacked, Crashed, and Floor Bool
#pragma warning disable 0649
    [SerializeField] private Animator anim;
#pragma warning restore 0649
    private bool playerEnter;
    private bool floor;
    // Use this for initialization

    public bool GetCrashed()
    {
        return playerEnter;
    }
    public bool GetFloor()
    {
        return floor;
    }

    private void OnEnable()
    {
        playerEnter = false;
        floor = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && (anim != null))
        {
            playerEnter = true;
            HandleAnimation();
        }
        if ((other.tag == "Floor" && anim != null) || (other.tag == "HiddenFloor" && anim != null))
        {
            floor = true;
            HandleAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Floor" && anim != null) || (other.tag == "HiddenFloor" && anim != null))
        {
            floor = false;
            HandleAnimation();
        }
    }

    private void HandleAnimation()
    {
        anim.SetBool("Crashed", playerEnter);
        anim.SetBool("Floor", floor);
    }
}
