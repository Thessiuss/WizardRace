using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
#pragma warning disable 0649
    [SerializeField] private Transform playerTransform;
#pragma warning restore 0649
    private float step = 25;

    private void Start()
    {
        step = GlobalGameManager.instance.GetMaxVelocity() + 5;
    }
    // Update is called once per frame
    void Update () {
        this.transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, Time.deltaTime * step);
	}
}
