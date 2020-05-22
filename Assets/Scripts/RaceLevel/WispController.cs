using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispController : MonoBehaviour {
    private List<GameObject> wispPrefabList;
    private List<GameObject> wispClusterList;
    private List<Vector2> wispPositions;

    private List<int> wispSpawnOrder;
    private List<ObjectPool> wispPools = new List<ObjectPool>();
    private GlobalGameManager ggm;
    private ScoreController scoreController;
    private string seed;
    private System.Random pseudoRandom;

    void Start ()
    {
        scoreController = GameObject.Find("ScoreController").GetComponent<ScoreController>();
        ggm = GlobalGameManager.instance;
        wispPrefabList = ggm.GetWispPrefabs();
        wispClusterList = ggm.GetWispClusters();
        wispPositions = ggm.GetWispPositions();
        wispSpawnOrder = ggm.GetWispSpawnOrder();
        seed = ggm.GetSeed();
        pseudoRandom = new System.Random(seed.GetHashCode());
        PopulatePool();
        PlaceClusters();
	}
	
    private void PopulatePool()
    {
        // TODO: change to if statement depending on size of prefab list.
        int count = wispPrefabList.Count * 5;
        for (int i = 0; i < wispPrefabList.Count; i++)
        {
            ObjectPool objP = ScriptableObject.CreateInstance<ObjectPool>();
            objP.Populate(count, wispPrefabList[i], this.gameObject, true);
            wispPools.Add(objP);
        }
    }

    private void PlaceClusters()
    {
        int difference = wispPositions.Count - wispSpawnOrder.Count;
        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                int newSpawnIndex = pseudoRandom.Next(0, wispClusterList.Count - 1);
                wispSpawnOrder.Add(newSpawnIndex);
            }
        }
        for (int i = 0; i < wispPositions.Count; i++)
        {
            Vector3 position = new Vector3(wispPositions[i].x, wispPositions[i].y, 0);
            Instantiate(wispClusterList[wispSpawnOrder[i]], position, wispClusterList[wispSpawnOrder[i]].transform.rotation, this.transform);
        }
    }

    // Make overload method for different colors, non-existant, null, and comparable colors
    public GameObject GetWisp(int index)
    {
        if (index > wispPools.Count - 1)
        {
            index = 0;
        }
        GameObject wisp = wispPools[index].GetPooledObject();
        return wisp;
    }

    // Called by WispSpawn once it's triggered.
    public void CollectWisp(int value)
    {
        scoreController.AddWisp(value);
    }
}
