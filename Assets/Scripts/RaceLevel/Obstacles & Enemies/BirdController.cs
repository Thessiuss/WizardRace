using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    // TODO: Make sure movement is set to true
    // TODO: Make sure rotation is set to true
#pragma warning disable 0649
    [SerializeField] private Animator anim;
    [SerializeField] private EnemyProxAnimController epac;
    [SerializeField] private MovementToPlayer mtp;
    [SerializeField] private Obstacle obs;              // Obstacle Script
    [SerializeField] private RotateToPlayer rtp;
    [SerializeField] private GameObject col;
#pragma warning restore 0649
    private Rigidbody rb;
    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        col.layer = 15;
        isAttacking = false;
    }

    private void Update()
    {
        
        if (epac.GetPlayerEnter() && !isAttacking)
        {
            isAttacking = true;
            mtp.active = true;
            rtp.active = true;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        if (!epac.GetPlayerEnter() && !rb.useGravity)
        {
            isAttacking = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            mtp.active = false;
            rtp.active = false;
        }
        if (!obs.IsAlive())
        {
            anim.SetBool("Crashed", true);
            col.layer = 19;
            rb.isKinematic = false;
            rb.useGravity = true;
            mtp.active = false;
            rtp.active = false;
        }
    }

}
