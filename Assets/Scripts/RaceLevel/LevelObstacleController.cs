using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObstacleController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject proximityTriggerParent;
#pragma warning restore 0649

    private int groundedLayerMask;
    private int obstaclePointer;
    private List<int> obstacleRandomSpawnPositions = new List<int>();
    private Transform playerTransform;
    private System.Random pseudoRandom;
    private GameObject terrainParent;

    #region GGM Data
    private GlobalGameManager ggm;
    private int levelLength;
    [Range(0, 100)]
    private float obstacleFrequency;
    private List<GameObject> obstaclePrefabs;
    private List<int> obstacleSpawnOrder = new List<int>();
    private List<ObjectPool> obstaclePrefabPool = new List<ObjectPool>();
    private float renderDistance;
    private string seed;
    private List<Vector3> specialObjectLocations = new List<Vector3>();
    private List<GameObject> specialObjectPrefabs;
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
        obstacleFrequency = ggm.GetObstacleFrequency();
        obstaclePrefabs = ggm.GetObstaclePrefabs();
        obstacleSpawnOrder = ggm.GetObstacleSpawnOrder();
        renderDistance = ggm.GetRenderDistance();
        seed = ggm.GetSeed();
        specialObjectLocations = ggm.GetSpecialObjectLocations();
        specialObjectPrefabs = ggm.GetSpecialObjectPrefabs();

        obstaclePointer = 0;
        playerTransform = GameObject.Find("Player").transform;
        pseudoRandom = new System.Random(seed.GetHashCode());
        // TODO: Adjust count for obstacle & obstacle difficulty
        int count = 3;
        if (obstaclePrefabs.Count > 0)
            PopulatePool(obstaclePrefabs, obstaclePrefabPool, count);
        PopulateRandomLocations();
        PopulateSpawnOrder();
        if (specialObjectPrefabs.Count > 0)
            PopulateLocatedObjects(terrainParent, specialObjectPrefabs, specialObjectLocations);
    }

    private void Update()
    {
        if ((obstaclePrefabs.Count > 0) && (obstaclePointer < obstacleRandomSpawnPositions.Count))
        {
            if (obstacleRandomSpawnPositions[obstaclePointer] - playerTransform.position.x < renderDistance - 4)
            {
                RaycastHit hit;
                Vector3 raycastOrigin = new Vector3(obstacleRandomSpawnPositions[obstaclePointer] - 3,
                    playerTransform.position.y + 20, 0);
                if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, 1000, groundedLayerMask))
                {
                    GameObject go = AddObstaclePrefab(obstacleSpawnOrder[obstaclePointer]);
                    go.transform.position = (hit.point);
                    go.SetActive(true);
                    obstaclePointer++;
                }
            }
        }
    }

    private GameObject AddObstaclePrefab(int index)
    {
        GameObject pooledObj = obstaclePrefabPool[index].GetPooledObject();
        if (pooledObj == null)
        {
            pooledObj = obstaclePrefabs[0];
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
        if (obstacleFrequency == 0)
        {
            return;
        }
        for (int i = 0; i < levelLength; i++)
        {
            float rng = pseudoRandom.Next(0, 1000);
            if (rng <= obstacleFrequency)
            {
                obstacleRandomSpawnPositions.Add(i);
            }
        }
    }

    private void PopulateSpawnOrder()
    {
        int moreSpawnOrder = obstacleRandomSpawnPositions.Count - obstacleSpawnOrder.Count + 1;
        if (obstaclePrefabs.Count == 0)
            return;
        for (int i = 0; i < moreSpawnOrder; i++)
        {
            int order = pseudoRandom.Next(0, obstaclePrefabs.Count);
            obstacleSpawnOrder.Add(order);
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
