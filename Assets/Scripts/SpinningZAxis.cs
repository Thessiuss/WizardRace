using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningYAxis : MonoBehaviour {
    public int rpm;
		
	void Update () {
        transform.Rotate(0, 6.0f * rpm * Time.deltaTime, 0);
	}
}
