using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActiveColliderAdjustment : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField] private BoxCollider boxCol;
    [SerializeField] private CapsuleCollider capCol;
    [SerializeField] private SphereCollider sphCol;
    [SerializeField] private Vector3 newColliderCenter;
    [SerializeField] private Vector3 newBoxSize;
    [SerializeField] private float newHeight = 0;
    [SerializeField] private float newRadius = 0;
#pragma warning restore 0649
    private bool isActive;
    private float heightOriginal = 0;
    private float radiusOriginal = 0;
    private Vector3 boxSizeOriginal = new Vector3();
    private Vector3 colliderCenterOriginal = new Vector3();

    private void Awake()
    {
        
        if (boxCol != null)
        {
            if (newColliderCenter.sqrMagnitude == 0)
            {
                newColliderCenter = boxCol.center;
            }
            boxSizeOriginal = boxCol.size;
            colliderCenterOriginal = boxCol.center;
        }
        if (capCol != null)
        {
            if (newColliderCenter.sqrMagnitude == 0)
            {
                newColliderCenter = capCol.center;
            }
            heightOriginal = capCol.height;
            radiusOriginal = capCol.radius;
            colliderCenterOriginal = capCol.center;
        }
        if (sphCol != null)
        {
            if (newColliderCenter.sqrMagnitude == 0)
            {
                newColliderCenter = sphCol.center;
            }
            radiusOriginal = sphCol.radius;
            colliderCenterOriginal = sphCol.center;
        }
    }

    public void OnEnable()
    {
        if (boxCol != null)
        {
            boxCol.size = newBoxSize;
            boxCol.center = newColliderCenter;
        }
        if (capCol != null)
        {
            capCol.center = newColliderCenter;
            capCol.radius = newRadius;
            capCol.height = newHeight;
        }
        if (sphCol != null)
        {
            sphCol.center = newColliderCenter;
            sphCol.radius = newRadius;
        }
        isActive = true;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    private void OnDisable()
    {
        if (boxCol != null)
        {
            boxCol.size = boxSizeOriginal;
            boxCol.center = colliderCenterOriginal;
        }
        if (capCol != null)
        {
            capCol.height = heightOriginal;
            capCol.radius = radiusOriginal;
            capCol.center = colliderCenterOriginal;
        }
        if (sphCol != null)
        {
            sphCol.radius = radiusOriginal;
            sphCol.center = colliderCenterOriginal;
        }
        isActive = false;
    }
}
