using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MovementToPlayer mtp;
    [SerializeField] private MovementOmni momni;
    [SerializeField] private GameObject uColArm;
    [SerializeField] private GameObject uColArmTip;
    [SerializeField] private GameObject lColArm;
    [SerializeField] private GameObject lColArmTip;
    [SerializeField] private GameObject darkParticles;
#pragma warning restore 0649
    private CapsuleCollider lCollider;
    private CapsuleCollider lTipCollider;
    private CapsuleCollider uCollider;
    private CapsuleCollider uTipCollider;
    private void Awake()
    {
        lCollider = lColArm.GetComponent<CapsuleCollider>();
        lTipCollider = lColArm.GetComponent<CapsuleCollider>();
        uCollider = lColArm.GetComponent<CapsuleCollider>();
        uTipCollider = lColArm.GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        anim.SetBool("Crashed", true); 
        uColArm.layer = 19;
        uColArmTip.layer = 19;
        lColArm.layer = 19;
        lColArmTip.layer = 19;
        rb.isKinematic = false;
        rb.useGravity = true;
        momni.isActive = false;
        mtp.active = false;
        darkParticles.SetActive(false);
    }
    private void OnDisable()
    {
        anim.SetBool("Crashed", false);
        uColArm.layer = 0;
        uColArmTip.layer = 0;
        lColArm.layer = 0;
        lColArmTip.layer = 0;
        rb.isKinematic = true;
        rb.useGravity = false;
        momni.isActive = true;
        mtp.active = true;
        darkParticles.SetActive(true);
    }
}
