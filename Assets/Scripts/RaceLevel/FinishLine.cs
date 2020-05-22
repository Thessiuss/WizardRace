using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private GlobalGameManager ggm;
    private int levelLength;
    // Start is called before the first frame update
    void Start()
    {
        ggm = GlobalGameManager.instance;
        levelLength = ggm.GetLevelLength();
        transform.position = new Vector3(levelLength, 0, 0);
    }
    
}
