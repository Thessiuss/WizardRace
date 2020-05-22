using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class LevelController : MonoBehaviour 
{
    // TODO: If the terrain is past the finish line, stop populating.
    public static bool isActive = false;

#pragma warning disable 0649
    [SerializeField] private GameObject crystalBlue;
    [SerializeField] private GameObject crystalGreen;
    [SerializeField] private GameObject crystalRed;
    [SerializeField] private GameObject frontTrigger;
    [SerializeField] private GameObject levelBoundaryRear;
    [SerializeField] private GameObject terrainParent;
    [SerializeField] private Text startingText;
#pragma warning restore 0649
    private GameObject locatorFront;
    private GameObject locatorRear;

    private int initialDelay;
    private int levelLength;
    private float renderDistance;
    private string seed;
    private GlobalGameManager ggm;
    private List<Vector2> crystalPos;
    
    private int terrainPointerFront = 0;
    private int terrainPointerRear = 0;
    private List<ObjectPool> terrainPrefabPool = new List<ObjectPool>();
    private List<GameObject> terrainPrefabs;
    private List<int> terrainSpawnOrder;
    
    private ScoreController scoreController;
    private System.Random pseudoRandom;
    private Transform playerPosition;
    private Vector3 locator1 = new Vector3(0, 0, 0);


    void Awake()
    {
        scoreController = GameObject.Find("ScoreController").GetComponent<ScoreController>();
        locatorFront = GameObject.Find("LevelController/TerrainParent/LocatorFront");
        locatorRear = GameObject.Find("LevelController/TerrainParent/LocatorRear");
        terrainPointerFront = 0;
        terrainPointerRear = 0;
    }

    void Start()
    {
        // TODO: load up splash screen while things load in background
        ggm = GlobalGameManager.instance;
        playerPosition = GameObject.Find("Player").transform;
        AcquireLevelStatistics();
        frontTrigger.transform.localPosition = new Vector3(renderDistance +2, 0, 0);
        int terrainCount = terrainPrefabs.Count;
        if (terrainCount < 7)
        {
            terrainCount = 8 - terrainCount;
        }
        else
        {
            terrainCount = 2;
        }
        PopulatePool(terrainPrefabs, terrainPrefabPool, terrainCount);
        BeginLevel();
    }

    void Update()
    {
        if (isActive)
        {
            if ((Mathf.Abs(locatorFront.transform.position.x - playerPosition.position.x)) <= renderDistance)
            {
                AddFrontTerrain();
            }
            if ((Mathf.Abs(playerPosition.position.x - locatorRear.transform.position.x)) < 28 && terrainPointerRear > 0)
            {
                AddRearTerrain();
            }
            if (terrainPointerRear <= 0 && !levelBoundaryRear.activeSelf)
            {
                levelBoundaryRear.SetActive(true);
            }
        }
    }

    private void AcquireLevelStatistics()
    {
        crystalPos = ggm.GetCrystalLocations();
        initialDelay = ggm.GetInitialDelay();
        levelLength = ggm.GetLevelLength();
        renderDistance = ggm.GetRenderDistance();
        terrainPrefabs = ggm.GetTerrainPrefabs();
        terrainSpawnOrder = ggm.GetTerrainSpawnOrder();
        // Handling the appropriate seed.
        seed = ggm.GetSeed();
        if (seed == string.Empty)
        {
            seed = "StandardSeed";
        }
        if (ggm.GetRandomSeed())
        {
            seed = DateTime.Now.Ticks.ToString();
        }
        pseudoRandom = new System.Random(seed.GetHashCode());
        float totalTerrainLength = 0;
        if (terrainSpawnOrder.Count >= 1)
        {
            if (terrainPrefabs.Count > 0)
            {
                for (int i = 0; i < terrainSpawnOrder.Count; i++)
                {

                    // see what the length is between child 0 and 1 for the level and add it up.
                    totalTerrainLength += Mathf.Abs(terrainPrefabs[terrainSpawnOrder[i]].transform.GetChild(0).transform.position.x -
                        terrainPrefabs[terrainSpawnOrder[i]].transform.GetChild(1).transform.position.x);
                }
                // It's +45 because we begin populating from the LocatorRear which is at -30, and we want ~15m past the finishline
                PopulateTerrainSpawnOrder(totalTerrainLength);
            }
            else
            {
                Debug.Log("Terrain Prefab pool doesn't have anything in it");
            }
        }
        else
        {
            PopulateTerrainSpawnOrder(totalTerrainLength);
        }

        // placing the LevelBoundary Deactivator so it shuts off when it's supposed to.
        levelBoundaryRear.transform.GetChild(2).transform.position = new Vector3(-19 +
            terrainPrefabs[terrainSpawnOrder[0]].transform.GetChild(1).transform.position.x, 0, 0);
    }

    // Instantiate prefab pools
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

    public void BeginLevel()
    {
        GenerateMap();
        startingText.gameObject.SetActive(true);
        InvokeRepeating("CountDown", 0.0f, 1f);
    }

    void CountDown()
    {
        if (initialDelay > 0)
        {
            startingText.text = initialDelay.ToString();
        }
        else if (initialDelay == 0)
        {
            startingText.text = "GO!";
            isActive = true;
        }
        else if (initialDelay <= -1)
        {
            startingText.gameObject.SetActive(false);
            CancelInvoke("CountDown");
        }
        initialDelay--;
    }

    // Called at the beginning of the level to popluate the terrain and instantiate Object Pools
    void GenerateMap()
    {
        if (crystalPos.Count < 3)
        {
            PopulateCrystals(Vector2.up, Vector2.left, Vector2.right);
        }
        else
        {
            PopulateCrystals(crystalPos[0], crystalPos[1], crystalPos[2]);
        }
        // populating special objects
        
        PopulateTile(terrainParent);
    }

    private void PopulateCrystals(Vector2 redPos, Vector2 greenPos, Vector2 bluePos)
    {
        crystalRed.transform.position = new Vector3 (redPos.x, redPos.y, 0);
        crystalGreen.transform.position = new Vector3(greenPos.x, greenPos.y, 0);
        crystalBlue.transform.position = new Vector3(bluePos.x, bluePos.y, 0);
    }

    private void PopulateTerrainSpawnOrder(float totalTerrainLength)
    {
        float time = 0;             // If the end of the level is too far, and it takes forever, break out.
        while (totalTerrainLength + 45 < levelLength)
        {
            // TEST: if it spawns it out of the index, the problem will be the .Add
            terrainSpawnOrder.Add(pseudoRandom.Next(0, terrainPrefabs.Count));
            totalTerrainLength += Mathf.Abs(
                terrainPrefabs[terrainSpawnOrder[terrainSpawnOrder.Count - 1]].transform.GetChild(0).transform.position.x -
                terrainPrefabs[terrainSpawnOrder[terrainSpawnOrder.Count - 1]].transform.GetChild(1).transform.position.x);
            time += Time.deltaTime;
            if (time > 10.0f)
                break;
        }
    }

    //The level is broken into tiles, and each tile is populated with Terrain & Obstacle Objects 
    private void PopulateTile(GameObject terrainParent)
    {
        int tempCounter = 0;
        while (Mathf.Abs(locatorFront.transform.position.x) <= renderDistance)
        {
            AddFrontTerrain();
            if (tempCounter > 10)
            {
                Debug.Log("Terrain is set up incorrectly.");
                break;
            }
            tempCounter++;
        }
    }
    

    #region TerrainHandlers
    private void AddFrontTerrain()
    {
        AddTerrain(true);
        terrainPointerFront++;
    }
    private void AddRearTerrain()
    {
        terrainPointerRear -= 1;
        AddTerrain(false);
        
    }
    private void AddTerrain(bool forward)
    {
        GameObject terrainObj;
        // Makes sure the index isn't out of range
        if (terrainSpawnOrder[terrainPointerFront] > (terrainPrefabs.Count - 1))
        {
            terrainSpawnOrder[terrainPointerFront] = terrainPrefabs.Count - 1;
            Debug.Log("terrain spawn index is out of range: AT");
        }
        if (terrainPointerFront > terrainSpawnOrder.Count - 1)
        {
            int i = pseudoRandom.Next(0, terrainPrefabs.Count);
            terrainSpawnOrder.Add(i);
        }
        // Populates the terrain piece in front of the currently activated ti
        if (forward)
        {
            
            terrainObj = AcquireTerrainPrefab(terrainSpawnOrder[terrainPointerFront]);
            locator1 = terrainObj.transform.GetChild(0).transform.position;
            terrainObj.transform.position = locatorFront.transform.position + terrainObj.transform.position - locator1;
            locatorFront.transform.position = terrainObj.transform.GetChild(1).transform.position;
        }
        // Populates the terrain piece behind the currently activated ones.
        else
        {
            terrainObj = AcquireTerrainPrefab(terrainSpawnOrder[terrainPointerRear]);
            locator1 = terrainObj.transform.GetChild(1).transform.position;
            terrainObj.transform.position = locatorRear.transform.position - (locator1 - terrainObj.transform.position);
            locatorRear.transform.position = terrainObj.transform.GetChild(0).transform.position;
        }
        terrainObj.SetActive(true);
    }

    private GameObject AcquireTerrainPrefab(int terrIndex)
    {
        // TODO: Break it up according to Front or Back.
        GameObject pooledObj = terrainPrefabPool[terrIndex].GetPooledObject();
        if (pooledObj == null)
        {
            pooledObj = terrainPrefabs[0];
            return pooledObj;
        }
        return pooledObj;
    }
    
    public void RemoveFrontTerrain(GameObject locatorRef)
    {
        locatorFront.transform.position = locatorRef.transform.position;
        locatorRef.transform.parent.gameObject.SetActive(false);
        terrainPointerFront -= 1;
    }
    public void RemoveRearTerrain(GameObject locatorRef)
    {
        locatorRear.transform.position = locatorRef.transform.position;
        locatorRef.transform.parent.gameObject.SetActive(false);
        terrainPointerRear++;
    }
    #endregion TerrainHandlers


    public void CollectRedCrystal()
    {
        crystalRed.SetActive(false);
    }
    public void CollectGreenCrystal()
    {
        crystalGreen.SetActive(false);
    }
    public void CollectBlueCrystal()
    {
        crystalBlue.SetActive(false);
    }

    // When the level has reached the distance needed, the effects will end and everything will be updated
    // Triggered by PlayerController OnTriggerEnter
    public void EndLevel()
    {
        isActive = false;
        scoreController.EndLevelCalculations();
    }

}
