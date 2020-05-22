using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    public Vector2 endPos;
    public Vector3 endRot;
    public Vector3 endScale;
    public Vector2 startPos;
    public Vector3 startRot;
    public Vector3 startScale;
    public float deltPosTime;
    public float deltRotTime;
    public float deltScaleTime;

#pragma warning disable 0649
    [SerializeField] private GameObject uiElement;
#pragma warning restore 0649
    private Vector3 currentRot;
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    private void OnEnable()
    {
        StartAnimation();
    }

    // Update is called once per frame
    public void StartAnimation()
    {
        uiElement.transform.localPosition = startPos;
        //uiElement.transform.localRotation = startRot;
        uiElement.transform.localScale = startScale;
        StartCoroutine("Animate");
    }

    private IEnumerator Animate()
    {
        bool posDone = false;
        bool rotDone = false;
        bool scaleDone = false;
        float posTime = 0;
        float rotTime = 0;
        float scaleTime = 0;
        while (!posDone | !rotDone | !scaleDone)
        {
            uiElement.transform.localPosition = Vector3.Lerp(
                startPos, endPos, posTime);

            //uiElement.transform.rotation 
            //uiElement.transform.localRotation = Quaternion.Lerp(
            //    startRot, endRot, rotTime);
            uiElement.transform.localScale = Vector3.Lerp(
                startScale, endScale, scaleTime); 
            yield return null;
            //time += Time.deltaTime / animationTime;
        }
    }
}
