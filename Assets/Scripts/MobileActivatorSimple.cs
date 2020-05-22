using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileActivatorSimple : MonoBehaviour {
    private GlobalGameManager ggm;

	void Start ()
    {
        ggm = GlobalGameManager.instance;
        if (!ggm.mobileBuild)
        {
            this.gameObject.SetActive(false);
        }
	}
	
}
