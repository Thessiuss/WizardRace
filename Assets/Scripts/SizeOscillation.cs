using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeOscillation : MonoBehaviour
{
    public float speed = 1;
    public float maxScaleMult = 1;
    public float minScaleMult = 1;
    public float offSet = 1;
    public bool linear = true;
    public bool [] axis = new bool [3];

    private float currentScale = 1;
    private float step;
    [Range (-1,1)]
    private int direction = 1;
    private Vector3 startingScale;
    private Vector3 axisVector = Vector3.zero;
    
    private void Start()
    {
        startingScale = this.transform.localScale;
        currentScale = offSet;
        SetVectorAxis();
    }

    void Update()
    {
        if ((currentScale >= maxScaleMult && direction > 0) || (currentScale <= minScaleMult && direction < 0))
        {
            direction = direction * -1;
        }
        if (linear)
        {
            ScaleLinear();
        }
        else
        {
            ScaleCurved();
        }
        this.transform.localScale = startingScale + (axisVector * (currentScale - 1));
    }

    private void ScaleLinear()
    {
        currentScale = currentScale + (direction * speed * Time.deltaTime* 0.1f);
    }

    private void ScaleCurved()
    {
        if (direction > 0)
        {
            step += speed * Time.deltaTime * 0.1f;
        }
        else
        {
            step -= speed * Time.deltaTime * 0.1f;
        }
    }

    private void SetVectorAxis()
    {
        if (axis [0])
        {
            axisVector += new Vector3(1,0,0);
        }
        if (axis [1])
        {
            axisVector += new Vector3(0, 1, 0);
        }
        if (axis [2])
        {
            axisVector += new Vector3(0, 0, 1);
        }
    }
}
