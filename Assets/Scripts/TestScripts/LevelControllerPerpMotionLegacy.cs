using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelControllerPerpMotionLegacy : MonoBehaviour {
    /*
    // TODO: Turn tiles into prefabs and use a List with a given length of them
    public GameObject scoreTile;
    public GameObject trailingLocator;
    public List<GameObject> terrainPrefabList;
    public List<GameObject> obstaclePrefabList;
    public List<GameObject> enemyPrefabList;
    public Text startingText;

    // TODO: create gameobject array and make sure each object has a specific tag to search by.
    // TODO: make it populate and run in reverse if need to
    // TODO: R&D how to shut off slide movements without being flagged as malware
    // TODO: Create Constructors for most of the public variables
    // TODO: set up the speed and accelleration numbers through physics
    // TODO: Create a scene or script that instantiates constructors in order
    // TODO: Many of these variables will need to be moved to private and using get/set methods

    public static float maxVelocity = 50;
    public static float minVelocity = 5;
    public static float terrainLength;
    public static float gravity = 50;
    public static bool isActive = true;
    public static int acceleration;
    [Range(0, 100)]
    public static int obstacleDifficulty;
    [Range(0, 100)]
    public static int enemyDifficulty;
    public static int levelSize;
    public static int initialDelay;
    public static bool useRandomSeed;
    public static string seed;
    private List<ObjectPool> terrainPrefabPool = new List<ObjectPool>();
    private List<ObjectPool> obstaclePrefabPool = new List<ObjectPool>();
    private List<ObjectPool> enemyPrefabPool = new List<ObjectPool>();
    private Rigidbody scoreTileRB;
    private System.Random pseudoRandom;
    private Vector3 locator1 = new Vector3(0, 0, 0);
    private Vector3 xDirection = new Vector3(1, 0, 0);
    [Range(0.0f, 0.99f)]
    private float penalty;


    // TODO: Make tiles an array instead of individually.
    void Awake()
    {
        scoreTileRB = scoreTile.GetComponent<Rigidbody>();
        // ----------------- Test Code ---------------------- //
        // -------------- End Test Code --------------------- //
    }


    void Start()
    {
        // TODO: load up splash screen while things load in background
        PopulateLevelStatistics();
        PopulatePool(terrainPrefabList, terrainPrefabPool);
        PopulatePool(obstaclePrefabList, obstaclePrefabPool);
        PopulatePool(enemyPrefabList, enemyPrefabPool);
        BeginLevel();
    }


    // Update is called once per frame
    void Update()
    {
        // ----------------- Test Code ---------------------- //
        // -------------- End Test Code --------------------- //
        if (isActive)
        {
            if (Mathf.Abs(scoreTile.transform.position.x) >= levelSize)
            {
                EndLevel();
            }
            if (trailingLocator.transform.position.x < terrainLength)
            {
                AddMoreTerrain(scoreTile);
            }
        }
    }


    void FixedUpdate()
    {
        //uniformTime = Time.deltaTime;

    }


    private void PopulateLevelStatistics()
    {
        maxVelocity = GlobalGameManager.GetMaxVelocity();
        minVelocity = GlobalGameManager.GetMinVelocity();
        acceleration = GlobalGameManager.GetAcceleration();
        gravity = GlobalGameManager.GetGravity();
        obstacleDifficulty = GlobalGameManager.GetObstacleDifficulty();
        enemyDifficulty = GlobalGameManager.GetEnemyDifficulty();
        levelSize = GlobalGameManager.GetLevelSize();
        terrainLength = GlobalGameManager.GetTerrainLength();
        initialDelay = GlobalGameManager.GetInitialDelay();
        useRandomSeed = GlobalGameManager.GetRandomSeed();
        seed = GlobalGameManager.GetSeed();

        Physics.gravity = Vector3.up * -gravity;
    }


    // Instantiate prefab pools
    private void PopulatePool(List<GameObject> prefabList, List<ObjectPool> objectPools)
    {
        for (int i = 0; i < prefabList.Count; i++)
        {
            ObjectPool objP = ScriptableObject.CreateInstance<ObjectPool>();
            objP.Populate(prefabList.Count + 1, prefabList[i], scoreTile, true);
            objectPools.Add(objP);
        }
    }


    // Called when the start button is pushed on the canvasUI
    public void BeginLevel()
    {
        GenerateMap();
        startingText.gameObject.SetActive(true);
        InvokeRepeating("CountDown", 0.0f, 1f);
        InvokeRepeating("IncreaseTileVelocity", initialDelay + 1, 0.25f);
    }


    void CountDown()
    {
        // Might need to change up initialDelay variable to a private variable that gets changed
        if (initialDelay > 0)
        {
            startingText.text = initialDelay.ToString();
        }
        else if (initialDelay == 0)
        {
            startingText.text = "GO!";
            SetTileVelocity();
            isActive = true;
        }
        else if (initialDelay < -2)
        {
            startingText.gameObject.SetActive(false);
            CancelInvoke("CountDown");
        }
        initialDelay--;
    }


    // TODO: Set up a penalty system based on the object it collides with rather than make it global
    public void SetTileVelocity()
    {

        //Starting velocity of the tile
        float velocityMagnitude = scoreTileRB.velocity.magnitude;
        if (velocityMagnitude < minVelocity)
        {
            scoreTileRB.velocity = -xDirection * minVelocity;
        }
        else
        {
            velocityMagnitude = (minVelocity * penalty) + (velocityMagnitude * (1 - penalty));
            scoreTileRB.velocity = -xDirection * velocityMagnitude;
        }
    }


    void IncreaseTileVelocity()
    {
        //Instead of setting the velocity, we apply a force to the tiles in order to move them appropriately

        if (scoreTileRB.velocity.magnitude < maxVelocity)
        {
            scoreTileRB.AddForce(-xDirection * acceleration);
        }
        if (scoreTileRB.velocity.magnitude > maxVelocity)
        {
            scoreTileRB.velocity = -xDirection * maxVelocity;
        }
    }


    void GenerateMap()
    {
        if (useRandomSeed)
        {
            seed = DateTime.Now.Ticks.ToString();
        }
        pseudoRandom = new System.Random(seed.GetHashCode());
        PopulateTile(scoreTile);
    }


    //The level is broken into tiles, and each tile is populated with Terrain & Obstacle Objects 
    //TODO: create an array for the objects to populate in each tile, then instead of instantiate/destroy, replace/remove.
    //TODO: Move instantiate into Start or Awake when creating object pools.
    //TODO: Do a check if locators are NULL and set out debug message
    void PopulateTile(GameObject tile)
    {
        while (trailingLocator.transform.position.x < terrainLength)
        {
            AddMoreTerrain(tile);
        }
    }


    // TODO: Will likely get rid of the return of GameObject when using object pooling.
    private GameObject AddTerrainPrefab(int index)
    {
        GameObject pooledObj = terrainPrefabPool[index].GetPooledObject();
        locator1 = pooledObj.transform.GetChild(0).transform.position;
        if (pooledObj == null)
        {
            pooledObj = terrainPrefabList[0];
            pooledObj.transform.position = trailingLocator.transform.position + pooledObj.transform.position - locator1;
            return pooledObj;
        }
        pooledObj.transform.position = trailingLocator.transform.position + pooledObj.transform.position - locator1;
        trailingLocator.transform.position = pooledObj.transform.GetChild(1).transform.position;
        // May need to spawn the pooled object that the specified rotation. ignoring for now.
        return pooledObj;
    }


    private GameObject AddObstaclePrefab(int oplc)
    {
        int index = pseudoRandom.Next(0, oplc);
        GameObject pooledObj = obstaclePrefabPool[index].GetPooledObject();
        if (pooledObj == null)
        {
            pooledObj = obstaclePrefabList[0];
            pooledObj.transform.position = trailingLocator.transform.position + pooledObj.transform.position - locator1;
            return pooledObj;
        }
        // TODO: Set up the position of the item to be according to a random position along the length of the terrain
        // Be sure to have multiple lengths set up
        pooledObj.transform.position = trailingLocator.transform.position;
        return pooledObj;
    }


    private GameObject AddEnemyPrefab(int eplc)
    {
        int index = pseudoRandom.Next(0, eplc);
        GameObject pooledObj = enemyPrefabPool[index].GetPooledObject();
        if (pooledObj == null)
        {
            pooledObj = enemyPrefabList[0];
            pooledObj.transform.position = trailingLocator.transform.position + pooledObj.transform.position - locator1;
            return pooledObj;
        }
        // TODO: Set up the position of the item to be according to a random position along the length of the terrain
        // Be sure to have multiple lengths set up
        pooledObj.transform.position = trailingLocator.transform.position;
        return pooledObj;
    }


    // TODO: Need to change this to velocity instead of messing around with position. It's causing the wizard to run 'through'
    // the meshes at fast velocities
    void AddMoreTerrain(GameObject tile)
    {
        int oplc = obstaclePrefabList.Count;
        int eplc = enemyPrefabList.Count;
        int terrainIndex = pseudoRandom.Next(0, terrainPrefabList.Count);
        GameObject terrainObj = AddTerrainPrefab(terrainIndex);
        terrainObj.SetActive(true);

        // Instantiating Obstacles to be dodged at whatever interval based on its difficulty
        int odCheck = pseudoRandom.Next(0, 100);
        if ((odCheck <= obstacleDifficulty) && (oplc > 0))
        {
            GameObject obstacleObj = AddObstaclePrefab(oplc);
            obstacleObj.SetActive(true);
        }

        // Instantiate Enemy 
        int edCheck = pseudoRandom.Next(0, 100);
        if ((edCheck <= enemyDifficulty) && (eplc > 0))
        {
            GameObject enemyObj = AddEnemyPrefab(eplc);
            enemyObj.gameObject.SetActive(true);
        }
    }


    // When the level has reached the distance needed, the effects will end and everything will be updated
    void EndLevel()
    {
        CancelInvoke("IncreaseTileVelocity");
        scoreTileRB.velocity = Vector3.zero;
        isActive = false;
    }


    //Level controller will most likely not handle interactions with objects, just with itself. keeping as placeholder for now
    private void OnTriggerEnter(Collider other)
    {
    }
    */
}
