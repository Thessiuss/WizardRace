using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispSpawn : MonoBehaviour {
    public WispController wispCont;
    public int wispIndex;
    public int wispValue;

    private GameObject wisp;

    private void Awake()
    {
        wispCont = GameObject.Find("WispController").GetComponent<WispController>();
    }

    private void OnEnable()
    {
        wisp = wispCont.GetWisp(wispIndex);
        if (wisp == null)
        {
            return;
        }
        wisp.transform.position = this.transform.position;
        wisp.SetActive(true);
    }
}