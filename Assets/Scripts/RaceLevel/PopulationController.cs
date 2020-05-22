using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopulationController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Animator modelAnimator;
    [HideInInspector] public bool employWorking;
    [HideInInspector] public int idleTime;

    [HideInInspector] public bool walkingAround;
    [HideInInspector] public float movementSpeed;
    [HideInInspector] public float widthX;
    [HideInInspector] public float widthY;
    [HideInInspector] public float widthZ;
    [HideInInspector] public float activeTime;
    [HideInInspector] public float restTime;
#pragma warning restore 0649
    private float timeCounter = 0;
    private Vector3 originalPos;
    private float posX;
    private float posY;
    private float posZ;

    private void OnEnable()
    {
        if (employWorking)
        {
            InvokeRepeating("EmployWorkingTrigger", idleTime, idleTime);
        }
    }

    private void Start()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        if (walkingAround)
        {
            timeCounter += Time.deltaTime * movementSpeed;
            posX = Mathf.Cos(timeCounter) * widthX;
            posZ = Mathf.Sin(timeCounter) * widthZ;

            transform.position = new Vector3(originalPos.x + posX, originalPos.y + posY, originalPos.z + posZ);

            if (movementSpeed >= 3)
            {
                modelAnimator.SetBool("Running", true);
            }
            else
            {
                modelAnimator.SetBool("Walking", true);
            }
        }
    }

    private void OnDisable()
    {
        CancelInvoke("EmployWorkingTrigger");
        timeCounter = 0;
    }

    private void EmployWorkingTrigger()
    {
        modelAnimator.SetTrigger("Attack");
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(PopulationController))]
public class PopulationController_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();         // for other non-HideInInspector fields
        PopulationController script = (PopulationController)target;
        script.employWorking = EditorGUILayout.Toggle("Employ Working", script.employWorking);
        if (script.employWorking)
        {
            EditorGUI.indentLevel++;
            script.idleTime = EditorGUILayout.IntField("Idle Time", script.idleTime);
            EditorGUI.indentLevel--;
        }

        script.walkingAround = EditorGUILayout.Toggle("Walking Around", script.walkingAround);
        if (script.walkingAround)
        {
            EditorGUI.indentLevel++;
            script.activeTime = EditorGUILayout.FloatField("Active Time", script.activeTime);
            script.restTime = EditorGUILayout.FloatField("Rest Time", script.restTime);
            script.movementSpeed = EditorGUILayout.FloatField("Movement Speed", script.movementSpeed);
            script.widthX = EditorGUILayout.FloatField("Width X", script.widthX);
            script.widthY = EditorGUILayout.FloatField("Width Y", script.widthY);
            script.widthZ = EditorGUILayout.FloatField("Width Z", script.widthZ);
            EditorGUI.indentLevel--;
        }
    }
}
#endif