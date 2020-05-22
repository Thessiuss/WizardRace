using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWSpinnerData : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private List<GameObject> rarePrefabs;
    [SerializeField] private List<GameObject> uncommonPrefabs;
    [SerializeField] private List<GameObject> commonPrefabs;
#pragma warning restore 0649
    private string seed;
    private System.Random pseudoRandom;
    private List<GameObject> spinner16 = new List<GameObject>();
    private List<GameObject> spinner12 = new List<GameObject>();
    private List<GameObject> spinner8 = new List<GameObject>();
    // TODO: Get spinner data from Playfab

    private void Awake()
    {
        seed = DateTime.Now.Day + "d" + DateTime.Now.Month + "m" + DateTime.Now.Year + "y";
        pseudoRandom = new System.Random(seed.GetHashCode());

        PopulateSpinner16();
        PopulateSpinner12();
        PopulateSpinner8();
    }

    private void PopulateSpinner16()
    {
        for (int i=0; i < 3; i++)
        {
            int prefabIndex = pseudoRandom.Next(0, rarePrefabs.Count);
            spinner16.Add(rarePrefabs[prefabIndex]);
        }
        for (int i=0; i<5; i++)
        {
            int prefabIndex = pseudoRandom.Next(0, uncommonPrefabs.Count);
            spinner16.Add(uncommonPrefabs[prefabIndex]);
        }
        for (int i = 0; i < 8; i++)
        {
            int prefabIndex = pseudoRandom.Next(0, commonPrefabs.Count);
            spinner16.Add(commonPrefabs[prefabIndex]);
        }
    }
    private void PopulateSpinner12()
    {
        for (int i = 0; i < 3; i++)
        {
            int prefabIndex = pseudoRandom.Next(0, rarePrefabs.Count);
            spinner12.Add(rarePrefabs[prefabIndex]);
        }
        for (int i = 0; i < 5; i++)
        {
            int prefabIndex = pseudoRandom.Next(0, uncommonPrefabs.Count);
            spinner12.Add(uncommonPrefabs[prefabIndex]);
        }
        for (int i = 0; i < 4; i++)
        {
            int prefabIndex = pseudoRandom.Next(0, commonPrefabs.Count);
            spinner12.Add(commonPrefabs[prefabIndex]);
        }
    }
    private void PopulateSpinner8()
    {
        for (int i = 0; i < 3; i++)
        {
            int prefabIndex = pseudoRandom.Next(0, rarePrefabs.Count);
            spinner8.Add(rarePrefabs[prefabIndex]);
        }
        for (int i = 0; i < 5; i++)
        {
            int prefabIndex = pseudoRandom.Next(0, uncommonPrefabs.Count);
            spinner8.Add(uncommonPrefabs[prefabIndex]);
        }
    }

    public List<GameObject> GetSpinner16()
    {
        return spinner16;
    }
    public List<GameObject> GetSpinner12()
    {
        return spinner12;
    }
    public List<GameObject> GetSpinner8()
    {
        return spinner8;
    }
}
