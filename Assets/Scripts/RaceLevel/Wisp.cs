using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    public int wispValue;

    private GameObject parent;
    private MovementOmni mo;
    private Transform player;
    private WispController wispCont;
    private MovementOmni movement;
    private GameObject child1;
    private Animator anim;
    private Renderer render;
    private float timeStep;
    private ScoreController sc;

    private bool moveToWizard;
    // Use this for initialization
    private void Awake()
    {
        parent = transform.parent.gameObject;
        sc = GameObject.Find("ScoreController").GetComponent<ScoreController>();
        mo = parent.GetComponent<MovementOmni>();
        player = GameObject.Find("Player").transform;
        wispCont = GameObject.Find("WispController").GetComponent<WispController>();
        child1 = parent.transform.GetChild(1).gameObject;
        anim = this.GetComponent<Animator>();
        render = this.GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        child1.SetActive(false);
        sc.AddWispTotal(wispValue);
        anim.enabled = true;
        render.enabled = true;
        mo.enabled = true;
        moveToWizard = false;
        timeStep = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToWizard)
        {
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, player.position, timeStep);
            timeStep += Time.deltaTime * 0.5f ;
        }
    }

    public void TriggerWisp()
    {
        anim.enabled = false;
        mo.enabled = false;
        // Makes the particle effect show up & start consuming the Wisp
        StartCoroutine("CaptureTransition");
    }

    public void CollectWisp()
    {
        // Make sure to turn back on the wisp_bounce and the 
        child1.SetActive(false);
        wispCont.CollectWisp(wispValue);
        parent.SetActive(false);
    }

    private IEnumerator CaptureTransition()
    {
        child1.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        render.enabled = false;
        moveToWizard = true;
        yield break;
        
    }
}
