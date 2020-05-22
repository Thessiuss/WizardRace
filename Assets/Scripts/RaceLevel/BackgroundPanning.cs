using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPanning : MonoBehaviour
{
    public Vector3 endPosition;

    private Vector3 startPosition;
    private GlobalGameManager ggm;
    private Transform playerPos;
    private Transform objectTransform;
    private float distanceRatio;
    private int levelLength;
    
    private void Start()
    {
        ggm = GlobalGameManager.instance;
        levelLength = ggm.GetLevelLength();
        playerPos = GameObject.Find("Player").transform;
        objectTransform = gameObject.transform;
        startPosition = gameObject.transform.position;
        if (levelLength <= 0)
        {
            levelLength = 10;
        }
    }

    private void Update()
    {
        distanceRatio = playerPos.position.x / levelLength;
        float xPos = (startPosition.x * (1 - distanceRatio)) + (endPosition.x * distanceRatio);
        objectTransform.position = new Vector3 (xPos, objectTransform.position.y, objectTransform.position.z);
    }
}
