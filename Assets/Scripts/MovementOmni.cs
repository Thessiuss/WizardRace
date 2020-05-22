using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The object this is attached to moves in a direction according to the public parameters
// Movement can be random and change directions
public class MovementOmni : MonoBehaviour {
    public float targetDistX = 0;
    public float targetDistY = 0;
    public float targetDistZ = 0;
    public float maxSpeed = 2;
    public float maxTime = 1;
    public bool randomDir = true;
    public bool continuousMovement = false;
    public bool rotateModel;
    public bool isActive = true;

    private Vector3 newPosition;
    private float xVelocity;
    private float yVelocity;
    private bool forwardFacing = true;
    private Quaternion startingRotation;
    private Quaternion targetRotation;

    // Awake was setting the newPosition to (0,0,0). Had to change it to Start for a delayed data update first.
    private void Awake()
    {
        startingRotation = transform.rotation;
    }

    private void OnEnable()
    {
        transform.rotation = startingRotation;
        forwardFacing = true;
        newPosition = transform.position;
        targetRotation = transform.rotation;

    }
	
	void Update ()
    {
        if (isActive)
        {
            // If it's close to its movement point, change directions.
            if (
                (targetDistX != 0) && (Mathf.Abs(transform.position.x - newPosition.x)) < 0.1f ||
                (targetDistY != 0) && (Mathf.Abs(transform.position.y - newPosition.y)) < 0.1f ||
                (targetDistZ != 0) && (Mathf.Abs(transform.position.z - newPosition.z)) < 0.1f
                )
            {
                SetNewPosition();
                if (rotateModel)
                {
                    AdjustQuaternion();
                }
            }
            if (!continuousMovement)
            {
                // Slowly move it toward the new position
                float newX = Mathf.SmoothDamp(transform.position.x, newPosition.x, ref xVelocity, maxTime, maxSpeed);
                float newY = Mathf.SmoothDamp(transform.position.y, newPosition.y, ref yVelocity, maxTime, maxSpeed);
                transform.position = new Vector3(newX, newY, transform.position.z);
                // Rotate it toward the targeted Quaternion
                if (rotateModel)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
                }
            }
            if (continuousMovement)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition, maxSpeed * Time.deltaTime);
            }
        }
        
        
    }

    // Wisp changes directions according to the parameters
    private void SetNewPosition()
    {
        
        if (randomDir)
        {
            newPosition = new Vector3((Random.Range(-targetDistX, targetDistX) + transform.localPosition.x),
                (Random.Range(-targetDistY, targetDistY) + transform.localPosition.y), 0);
        }
        else if (!continuousMovement)
        {
            newPosition = new Vector3(transform.position.x + targetDistX, transform.position.y + targetDistY, 0);
        }
        else if (continuousMovement)
        {
            float newX = transform.position.x + targetDistX;
            float newY = transform.position.y + targetDistY;
            float newZ = transform.position.z + targetDistZ;
            newPosition = new Vector3(newX, newY, newZ);
            Debug.Log("Set New Position: ");

            Debug.Log("New Position: " + newPosition);
        }
        // May need to change this to a raycast to make sure it's above ground.
        if (newPosition.y < 0.3f)
        {
            newPosition.y = 0.35f;
        }
    }

    private void AdjustQuaternion()
    {       
        if ((newPosition.x > transform.position.x) && !forwardFacing)
        {
            targetRotation.eulerAngles = new Vector3 (targetRotation.x, 
                startingRotation.eulerAngles.y, targetRotation.z);
            forwardFacing = true;
        }
        else if ((newPosition.x < transform.position.x) && forwardFacing)
        {
            targetRotation.eulerAngles = new Vector3(targetRotation.x,
                startingRotation.eulerAngles.y + 180, targetRotation.z);
            forwardFacing = false;
        }
    }
}
