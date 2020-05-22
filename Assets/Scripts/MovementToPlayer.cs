using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementToPlayer : MonoBehaviour
{
    public bool active;
#pragma warning disable 0649
    [SerializeField] private float maxSpeed;
    [SerializeField] private bool moveXDir;
    [SerializeField] private bool moveYDir;
    [SerializeField] private bool moveZDir;
#pragma warning restore 0649
    private Transform playerTransform;
    private Vector3 newPosition;
    private float moveX = 0;
    private float moveY = 0;
    private float moveZ = 0;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    private void OnEnable()
    {
        moveX = transform.position.x;
        moveY = transform.position.y;
        moveZ = transform.position.z;
    }

    private void Update()
    {
        if (active)
        {
            if (moveXDir)
            {
                moveX = playerTransform.position.x;
            }
            else
            {
                moveX = transform.position.x;
            }
            if (moveYDir)
            {
                moveY = playerTransform.position.y;
            }
            else
            {
                moveY = transform.position.y;
            }
            if (moveZDir)
            {
                moveZ = playerTransform.position.z;
            }
            else
            {
                moveZ = transform.position.z;
            }
            newPosition = new Vector3(moveX, moveY, moveZ);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, maxSpeed * Time.deltaTime);
        }
    }
}
