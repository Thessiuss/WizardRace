using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    public bool active;

#pragma warning disable 0649
    [SerializeField] private bool rotXDir;
    [SerializeField] private bool rotYDir;
    [SerializeField] private bool rotZDir;
#pragma warning restore 0649
    private Transform playerTransform;
    private Quaternion targetRotation;
    private Vector3 openAxis = new Vector3(0, 0, 0);
    private Vector3 opResult;
    
    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        // If we are rotating along the X axis, then we look at the Y and Z coordinates.
        if (rotXDir && !rotYDir && !rotZDir)
            openAxis += Vector3.up + Vector3.forward;

        // If we are rotating along the y axis, then we need to look at the X and Z coordinates.
        else if (!rotXDir && rotYDir && !rotZDir)
            openAxis += Vector3.right + Vector3.forward;

        // If we're rotating along the Z axis, we need to look at the X and Y coordinates.
        else if (!rotXDir && !rotYDir && rotZDir)
            openAxis += Vector3.right + Vector3.up;

        // If we're rotating along more than 1 axis, then it's open rotation.
        else
        {
            rotXDir = true;
            rotYDir = true;
            rotZDir = true;
        }
    }

    // May have to set an OnDisable or OnEnable to reset certain values.

    private void Update()
    {
        if (active)
        {
            if (rotXDir && rotYDir && rotZDir)
            {
                targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
            }
            else
            {
                opResult = new Vector3(playerTransform.position.x * openAxis.x, playerTransform.position.y * openAxis.y, playerTransform.position.z * openAxis.z);
                transform.LookAt(opResult);
            }
        }
    }
}
