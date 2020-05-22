using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemyController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject proximityTriggerParent;
#pragma warning restore 0649

    private int groundedLayerMask;
    private int enemyPointer;
    private List<int> enemyRandomSpawnPositions = new List<int>();
    private Transform playerTransform;
    private System.Random pseudoRandom;
    private GameObject terrainParent;

    #region GGM Data
    private GlobalGameManager ggm;
    private int levelLength;
    [Range(0, 100)]
    private float enemyFrequency;
    private List<GameObject> enemyPrefabs;
    private List<int> enemySpawnOrder = new List<int>();
    private List<ObjectPool> enemyPrefabPool = new List<ObjectPool>();
    private List<GameObject> enemySpecificPrefabs = new List<GameObject>();
    private List<Vector3> enemySpawnPositions = new List<Vector3>();
    private float renderDistance;
    private string seed;
    #endregion GGM Data

    private void Awake()
    {
        terrainParent = GameObject.Find("LevelController/TerrainParent");
        groundedLayerMask = (1 << 0) | (1 << 11) | (1 << 16) | (1 << 17);
    }

    private void Start()
    {
        ggm = GlobalGameManager.instance;
        levelLength = ggm.GetLevelLength();
        enemyFrequency = ggm.GetEnemyFrequency();
        enemyPrefabs = ggm.GetEnemyPrefabs();
        enemySpawnOrder = ggm.GetEnemySpawnOrder();
        renderDistance = ggm.GetRenderDistance();
        seed = ggm.GetSeed();
        enemySpawnPositions = ggm.GetSpecialObjectLocations();
        enemySpecificPrefabs = ggm.GetEnemySpecificPrefabs();

        enemyPointer = 0;
        playerTransform = GameObject.Find("Player").transform;
        pseudoRandom = new System.Random(seed.GetHashCode());
        // TODO: Adjust count for obstacle & obstacle difficulty
        int count = 3;
        if (enemyPrefabs.Count > 0)
            PopulatePool(enemyPrefabs, enemyPrefabPool, count);
        PopulateRandomLocations();
        PopulateSpawnOrder();
        if (enemySpecificPrefabs.Count > 0)
            PopulateLocatedObjects(terrainParent, enemySpecificPrefabs, enemySpawnPositions);
    }

    private void Update()
    {
        if ((enemyPrefabs.Count > 0) && (enemyPointer < enemyRandomSpawnPositions.Count))
        {
            if (enemyRandomSpawnPositions[enemyPointer] - playerTransform.position.x < renderDistance - 4)
            {
                RaycastHit hit;
                Vector3 raycastOrigin = new Vector3(enemyRandomSpawnPositions[enemyPointer] - 3,
                    playerTransform.position.y + 20, 0);
                if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 1000, groundedLayerMask))
                {
                    GameObject go = AddEnemyPrefab(enemySpawnOrder[enemyPointer]);
                    go.transform.position = (hit.point);
                    go.SetActive(true);
                    enemyPointer++;
                }
            }
        }
    }

    private GameObject AddEnemyPrefab(int index)
    {
        GameObject pooledObj = enemyPrefabPool[index].GetPooledObject();
        if (pooledObj == null)
        {
            pooledObj = enemyPrefabs[0];
            return pooledObj;
        }
        return pooledObj;
    }

    private void PopulatePool(List<GameObject> prefabList, List<ObjectPool> objectPools, int count)
    {
        for (int i = 0; i < prefabList.Count; i++)
        {
            // Finding out the number of prefabs to instantiate based on number of terrain objects
            ObjectPool objP = ScriptableObject.CreateInstance<ObjectPool>();
            objP.Populate(count, prefabList[i], terrainParent, true);
            objectPools.Add(objP);
        }
    }

    private void PopulateRandomLocations()
    {
        if (enemyFrequency == 0)
        {
            return;
        }
        for (int i = 0; i < levelLength; i++)
        {
            float rng = pseudoRandom.Next(0, 1000);
            if (rng >= (1000 - enemyFrequency))
            {
                enemyRandomSpawnPositions.Add(i);
            }
        }
    }

    private void PopulateSpawnOrder()
    {
        int moreSpawnOrder = enemyRandomSpawnPositions.Count - enemySpawnOrder.Count +1;
        if (enemyPrefabs.Count == 0)
            return;
        for (int i = 0; i < moreSpawnOrder; i++)
        {
            int order = pseudoRandom.Next(0, enemyPrefabs.Count);
            enemySpawnOrder.Add(order);
        }
    }

    private void PopulateLocatedObjects(GameObject parent, List<GameObject> prefabList, List<Vector3> prefabLocations)
    {
        int count = 0;
        count = prefabList.Count >= prefabLocations.Count ? prefabList.Count : prefabLocations.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                // proximity parent
                GameObject pp = Instantiate(proximityTriggerParent, prefabLocations[i], proximityTriggerParent.transform.rotation,
                    parent.transform);
                pp.GetComponent<ProximityEnabler>().SetRadius(renderDistance);
                GameObject go = Instantiate(prefabList[i], pp.transform);
                go.SetActive(false);
            }
        }
    }
}
