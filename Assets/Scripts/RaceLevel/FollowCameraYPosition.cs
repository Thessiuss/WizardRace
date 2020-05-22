using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraYPosition : MonoBehaviour
{
    public float followPercent;
#pragma warning disable 0649
    [SerializeField] private Transform cameraTransform;
#pragma warning restore 0649
    private Vector3 startPos;
    private float CameraStartPosY;
    private float posY;

    private void Start()
    {
        startPos = this.transform.position;
        CameraStartPosY = cameraTransform.position.y;
    }

    private void Update()
    {
        posY = ((cameraTransform.position.y - CameraStartPosY) * followPercent);
        this.transform.position = new Vector3(this.transform.position.x, startPos.y + posY, this.transform.position.z);
    }
}
