using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class LevelControllerBackup : MonoBehaviour
{
   /*
    
    // TODO: Turn tiles into prefabs and use a List with a given length of them
    public GameObject tile1;
    public GameObject tile2;
    public GameObject tile3;
    public GameObject scoreTile;
    public List<GameObject> terrainPrefabList;
    public GameObject[] obstaclePrefab;
    public GameObject[] enemyPrefab;
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
    public static int acceleration;

    public static float gravity = 50;
    [Range(0, 100)]
    public static int obstacleDifficulty;
    [Range(0, 100)]
    public static int enemyDifficulty;
    public static int levelSize;
    public static int terrainObjectsPerTile;
    public static int initialDelay;
    public static bool useRandomSeed;
    public static string seed;

    // ----------------- Test Code ---------------------- //

    // -------------- End Test Code --------------------- //



    // locator 2 is used as the front & back snapping point for the terrain
    private GameObject tileTerrain1, tileTerrain2, tileTerrain3;
    private Transform tileLocator1, tileLocator2, tileLocator3;
    private Rigidbody scoreTileRB;
    private System.Random pseudoRandom;
    private Vector3 origin = new Vector3(0, 0, 0);
    private Vector3 locator1 = new Vector3(0, 0, 0);
    private Vector3 locator2 = new Vector3(0, 0, 0);
    private Vector3 xDirection = new Vector3(1, 0, 0);
    [Range(0.0f, 0.99f)]
    private float penalty;
    private bool isActive = true;


    // TODO: Make tiles an array instead of individually.
    void Awake()
    {
        //we're populating the terrain in the tile, not the tile itself
        tileLocator1 = tile1.transform.GetChild(0).transform;
        tileLocator2 = tile2.transform.GetChild(0).transform;
        tileLocator3 = tile3.transform.GetChild(0).transform;

        //Getting reference to the locator transforms for each tile, so that setting transforms makes it easier
        tileTerrain1 = tile1.transform.GetChild(1).gameObject;
        tileTerrain2 = tile2.transform.GetChild(1).gameObject;
        tileTerrain3 = tile3.transform.GetChild(1).gameObject;

        scoreTileRB = scoreTile.GetComponent<Rigidbody>();
        // ----------------- Test Code ---------------------- //
        // -------------- End Test Code --------------------- //
    }


    void Start()
    {
        PopulateLevelStatistics();
        BeginLevel();
    }


    // Update is called once per frame
    void Update()
    {
        // ----------------- Test Code ---------------------- //
        // -------------- End Test Code --------------------- //
        if (isActive)
        {
            if (scoreTile.transform.position.x <= -levelSize)
            {
                EndLevel();
            }
            //We are moving the entire tiles, and then repositioning them, not just the terrain, but the entire tile
            RepositionTile();
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
        terrainObjectsPerTile = GlobalGameManager.GetTerrainObjectsPerTile();
        initialDelay = GlobalGameManager.GetInitialDelay();
        useRandomSeed = GlobalGameManager.GetRandomSeed();
        seed = GlobalGameManager.GetSeed();

        Physics.gravity = Vector3.up * -gravity;
    }


    // Called when the start button is pushed on the canvasUI
    public void BeginLevel()
    {
        isActive = true;
        startingText.gameObject.SetActive(true);
        InvokeRepeating("CountDown", 0.0f, 1f);
        InvokeRepeating("IncreaseTileVelocity", initialDelay + 1, 0.25f);
        GenerateMap();
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
        PopulateTile(tile1, tileTerrain1, tileLocator1, scoreTile.transform);
        PopulateTile(tile2, tileTerrain2, tileLocator2, tileLocator1);
        PopulateTile(tile3, tileTerrain3, tileLocator3, tileLocator2);
        //TODO: make a single method call
    }


    //The level is broken into tiles, and each tile is populated with Terrain & Obstacle Objects 
    //TODO: create an array for the objects to populate in each tile, then instead of instantiate/destroy, replace/remove.
    //TODO: Move instantiate into Start or Awake when creating object pools.
    //TODO: Do a check if locators are NULL and set out debug message
    void PopulateTile(GameObject tile, GameObject tileTerrain, Transform tileLocator, Transform previousTileLocator)
    {

        //tbal = Terrain Prefab Array Length
        //opal = Obstacle Prefab Array Length
        int tpal = terrainPrefabList.Count;
        int opal = obstaclePrefab.Length;
        int epal = enemyPrefab.Length;
        tile.transform.position = previousTileLocator.position;
        locator2 = previousTileLocator.position;

        //This is the loop that populates the terrain in each tile
        for (int i = 0; i < terrainObjectsPerTile; i++)
        {
            int terrainPrefabIndex = pseudoRandom.Next(0, tpal);
            origin = terrainPrefabList[terrainPrefabIndex].transform.position;
            locator1 = terrainPrefabList[terrainPrefabIndex].transform.GetChild(0).transform.position;

            //Snapping the new object after the position of the previous object (each object must have 2 locators)
            var terrainObject = Instantiate(terrainPrefabList[terrainPrefabIndex],
                (locator2 + origin - locator1), terrainPrefabList[terrainPrefabIndex].transform.rotation);
            locator2 = terrainObject.transform.GetChild(1).transform.position;
            tileLocator.position = locator2;

            //Setting the new object to it's parent tile, so that it moves with it.
            terrainObject.transform.parent = tileTerrain.transform;

            // Instantiating Obstacles to be dodged at whatever interval based on its difficulty
            int odCheck = pseudoRandom.Next(0, 100);
            if ((odCheck <= obstacleDifficulty) && (opal > 0))
            {
                int obstaclePrefabIndex = pseudoRandom.Next(0, opal);
                origin = obstaclePrefab[obstaclePrefabIndex].transform.position;
                var obstacleObject = Instantiate(obstaclePrefab[obstaclePrefabIndex],
                    (locator2 + origin), obstaclePrefab[obstaclePrefabIndex].transform.rotation);
                obstacleObject.transform.parent = tileTerrain.transform;
            }

            // Instantiate Enemy 
            int edCheck = pseudoRandom.Next(0, 100);
            if ((edCheck <= enemyDifficulty) && (epal > 0))
            {
                int enemyPrefabIndex = pseudoRandom.Next(0, epal);
                origin = enemyPrefab[enemyPrefabIndex].transform.position;
                var enemyObject = Instantiate(enemyPrefab[enemyPrefabIndex],
                    (locator2 + origin), enemyPrefab[enemyPrefabIndex].transform.rotation);
                enemyObject.transform.parent = tileTerrain.transform;
            }

        }
        //prepare locator 2 so that the next time we either move tiles, or repopulate terrain, the locator is ready

    }


    // When the level has reached the distance needed, the effects will end and everything will be updated
    void EndLevel()
    {
        CancelInvoke("IncreaseTileVelocity");
        scoreTileRB.velocity = Vector3.zero;
        isActive = false;
    }


    //TODO: Need to change this to velocity instead of messing around with position. It's causing the wizard to run 'through'
    // the meshes at fast velocities
    void RepositionTile()
    {
        //TODO: Change the tile to move to the last locator
        //TODO: make a prepare tile Method
        //Going to need to get the position of the last item placed and it's locator before i slap the tile to the next one
        if (tileLocator1.position.x < -20)
        {
            ClearTile(tileTerrain1);
            locator2 = tileLocator3.position;
            PopulateTile(tile1, tileTerrain1, tileLocator1, tileLocator3);
        }
        if (tileLocator2.position.x < -20)
        {
            ClearTile(tileTerrain2);
            locator2 = tileLocator1.position;
            PopulateTile(tile2, tileTerrain2, tileLocator2, tileLocator1);
        }
        if (tileLocator3.transform.position.x < -20)
        {
            ClearTile(tileTerrain3);
            locator2 = tileLocator2.position;
            PopulateTile(tile3, tileTerrain3, tileLocator3, tileLocator2);
        }
    }


    //TODO: Needs to be optimized with object pooling and less calling of destroy
    void ClearTile(GameObject tileTerrain)
    {
        foreach (Transform childTransform in tileTerrain.transform)
        {
            childTransform.gameObject.SetActive(false);
        }
    }


    //Level controller will most likely not handle interactions with objects, just with itself. keeping as placeholder for now
    private void OnTriggerEnter(Collider other)
    {
    }

    */

}



