using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningXAxis : MonoBehaviour {
    public int rpm;

    void Update()
    {
        transform.Rotate(6.0f * rpm * Time.deltaTime, 0, 0);
    }

}
