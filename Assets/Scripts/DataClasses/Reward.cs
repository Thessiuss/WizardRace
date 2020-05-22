using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject prefab;
    [SerializeField] private string objectName;
    [SerializeField] private string playfabName;
    [SerializeField] private string description;
    [SerializeField] private int[] quantity;
    [SerializeField] private bool randomQuantity;
    [SerializeField] private Vector3 scaleAdjust = new Vector3 (1,1,1);
#pragma warning restore 0649
    public GameObject GetPrefab()
    {
        return prefab;
    }
    public string GetObjectName()
    {
        return objectName;
    }
    public string GetPlayfabName()
    {
        return playfabName;
    }
    public string GetDescription()
    {
        return description;
    }
    public int[] GetQuantity()
    {
        return quantity;
    }
    public bool GetRandomQuantity()
    {
        return randomQuantity;
    }
    public Vector3 GetScaleAdjust()
    {
        return scaleAdjust;
    }
}
