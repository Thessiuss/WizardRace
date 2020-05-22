using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableSetActive : MonoBehaviour
{
    public List<GameObject> turnOnList;

    private void OnEnable()
    {
        for (int i = 0; i < turnOnList.Count; i++)
        {
            turnOnList [i].gameObject.SetActive(true);
        }
    }
}
