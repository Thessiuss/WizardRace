using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour {

    public static GlobalGameManager instance = null;
    // -------------- Maintenance Variables -------------- //
    // ----------------- Spell Variables ----------------- //


    // ----------------- Level Variables ----------------- //
    [SerializeField] private string levelName;
    [SerializeField] private string levelPerformance;
    [SerializeField] private int initialDelay;
    [SerializeField] private float renderDistance;
    [SerializeField] private float gravity;
    [SerializeField] private float fallMult;
    [SerializeField] private float lowJumpMult;
    [SerializeField] private bool useRandomSeed;
    [SerializeField] private string seed;
    [SerializeField] private int scoreCap;
    [SerializeField] private float[] scoreMarks;
    [SerializeField] private int levelLength;
    [SerializeField] private List<Vector2> crystalLocations;
    [Range(0, 500)]
    [SerializeField] private int enemyFrequency;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<int> enemySpawnOrder;
    [SerializeField] private List<GameObject> enemySpecificPrefabs;
    [SerializeField] private List<Vector3> enemySpawnPositions;
    [Range(0, 500)]
    [SerializeField] private int obstacleFrequency;
    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private List<int> obstacleSpawnOrder;
    [SerializeField] private List<Vector3> obstacleSpawnPositions;
    [SerializeField] private List<GameObject> specialObjectPrefabs;
    [SerializeField] private List<Vector3> specialObjectLocations;
    [SerializeField] private List<GameObject> terrainPrefabs;
    [SerializeField] private List<int> terrainSpawnOrder;
    [SerializeField] private List<GameObject> wispPrefabs;
    [SerializeField] private List<GameObject> wispClusters;
    [SerializeField] private List<Vector2> wispPositions;
    [SerializeField] private List<int> wispSpawnOrder;
    //TODO: Need to set up a Terrain Prefab List, so that levels can pull their data/prefabs from here
    //TODO: List of level scores that have highest score and previous score
    //TODO: Score values for each level    

    //TODO: Create Currency system. Make sure to add time stamps & time locked progression
    public bool mobileBuild;

    private LevelData levelData;
    private Notification notification;
    private RewardGroup completion;
    private RewardGroup firstTime;
    private RewardGroup mastery;
    private PlayerData playerData;
    private SpellData spellData;

    // -------------- Maintenance Variable Get & Set -------------- //

    #region Spell Variables
    public void SetSpellLevel(int level) {
        if (spellData.spellValues.Count < 1)
        {
            spellData.spellValues.Add(level);
        }
        else
            spellData.spellValues[0] = level;
    }
    public int GetSpellLevel() {
         // Will always at least have a valid number
        if (spellData.spellValues [0] <= 0)
        {
            return 0;
        }
        if (spellData.spellValues.Count < 1)
        {
            return 0;
        }
        return Mathf.RoundToInt(spellData.spellValues[0]);
    }
    public void SetSpellMultiplier(float multiplier) {
        spellData.spellMultiplier = multiplier;
    }
    public float GetSpellMultiplier() {
        return spellData.spellMultiplier;
    }
    public void SetSpellMultiplierCap(float cap) {
        spellData.spellMultiplierCap = cap;
    }
    public float GetSpellMultiplierCap() {
        return spellData.spellMultiplierCap;
    }
    public void SetSpell(GameObject spell) {
        spellData.spell = spell;
    }
    public GameObject GetSpell() {
        return spellData.spell;
    }
    public void SetStaff(GameObject staff) {
        spellData.staff = staff;
    }
    public GameObject GetStaff() {
        return spellData.staff;
    }
    #endregion Spell Variables

    #region Level Variables
    public void SetLevelData(LevelData ld)
    {
        levelName = ld.GetLevelName();
        levelPerformance = ld.GetLevelPerformance();
        initialDelay = ld.GetInitialDelay();
        renderDistance = ld.GetRenderDistance();
        gravity = ld.GetGravity();
        fallMult = ld.GetFallMult();
        lowJumpMult = ld.GetLowJumpMult();
        useRandomSeed = ld.GetUseRandomSeed();
        seed = ld.GetSeed();
        scoreCap = ld.GetScoreCap();
        scoreMarks = ld.GetScoreMarks();
        levelLength = ld.GetLevelLength();
        crystalLocations = ld.GetCrystalLocations();
        enemyFrequency = ld.GetEnemyFrequency();
        enemyPrefabs = ld.GetEnemyPrefabs();
        enemySpawnOrder = ld.GetEnemySpawnOrder();
        enemySpecificPrefabs = ld.GetEnemySpecificPrefabs();
        enemySpawnPositions = ld.GetEnemySpawnPositions();
        obstacleFrequency = ld.GetObstacleFrequency();
        obstaclePrefabs = ld.GetObstaclePrefabs();
        obstacleSpawnOrder = ld.GetObstacleSpawnOrder();
        obstacleSpawnPositions = ld.GetObstacleSpawnPositions();
        specialObjectPrefabs = ld.GetSpecialObjectPrefabs();
        specialObjectLocations = ld.GetSpecialObjectLocations();
        terrainPrefabs = ld.GetTerrainPrefabs();
        terrainSpawnOrder = ld.GetTerrainSpawnOrder();
        wispPrefabs = ld.GetWispPrefabs();
        wispClusters = ld.GetWispClusters();
        wispPositions = ld.GetWispPositions();
        wispSpawnOrder = ld.GetWispSpawnOrder();
    }
    public float GetGravity() {
        return gravity;
    }
    public float GetFallMult() {
        return fallMult;
    }
    public float GetLowJumpMult()
    {
        return lowJumpMult;
    }
    public int GetEnemyFrequency() 
    {
        return enemyFrequency;
    }
    public List<GameObject> GetEnemySpecificPrefabs()
    {
        return enemySpecificPrefabs;
    }
    public List<int> GetEnemySpawnOrder()
    {
        return enemySpawnOrder;
    }
    public List<Vector3> GetEnemySpawnPositions()
    {
        return enemySpawnPositions;
    }
    public int GetObstacleFrequency()
    {
        return obstacleFrequency;
    }
    public List<int> GetObstacleSpawnOrder()
    {
        return obstacleSpawnOrder;
    }
    public List<Vector3> GetObstacleSpawnPositions()
    {
        return obstacleSpawnPositions;
    }
    public List<Vector3> GetSpecialObjectLocations()
    {
        return specialObjectLocations;
    }
    public List<GameObject> GetSpecialObjectPrefabs()
    {
        return specialObjectPrefabs;
    }
    public int GetLevelLength() 
    {
        return levelLength;
    }
    public float GetRenderDistance() 
    {
        return renderDistance;
    }
    public int GetInitialDelay() 
    {
        return initialDelay;
    }
    public List<int> GetTerrainSpawnOrder()
    {
        return terrainSpawnOrder;
    }
    public List<int> GetWispSpawnOrder()
    {
        return wispSpawnOrder;
    }
    public bool GetRandomSeed() 
    {
        return useRandomSeed;
    }
    public string GetLevelName()
    {
        return levelName;
    }
    public string GetSeed() 
    {
        return seed;
    }
    public List<Vector2> GetWispPositions() 
    {
        return wispPositions;
    }
    public List<GameObject> GetWispPrefabs() 
    {
        return wispPrefabs;
    }
    public List<GameObject> GetWispClusters() 
    {
        return wispClusters;
    }
    public List<GameObject> GetTerrainPrefabs() 
    {
        return terrainPrefabs;
    }
    public List<GameObject> GetEnemyPrefabs() 
    {
        return enemyPrefabs;
    }
    public List<GameObject> GetObstaclePrefabs() 
    {
        return obstaclePrefabs;
    }
    public List<Vector2> GetCrystalLocations() 
    {
        return crystalLocations;
    }
    public string GetLevelPerformance() 
    {
        return levelPerformance;
    }
    public void SetLevelPerformance(string setLevelPerformance)
    {
        levelPerformance = setLevelPerformance;
    }
    #endregion Level Variables

    #region Level Rewards
    public void SetCompletionRewards(List<string> setSize, List<string> setColor, List<int> setQuantity){
        completion.size = setSize;
        completion.color = setColor;
        completion.quantity = setQuantity;
    }
    public void SetFirstTimeRewards(List<string> setSize, List<string> setColor, List<int> setQuantity) {
        firstTime.size = setSize;
        firstTime.color = setColor;
        firstTime.quantity = setQuantity;
    }
    public void SetMasteryRewards(List<string> setSize, List<string> setColor, List<int> setQuantity)
    {
        mastery.size = setSize;
        mastery.color = setColor;
        mastery.quantity = setQuantity;
    }
    #endregion Level Rewards

    // TODO: Set up player variables to be read only and have the set not be public.
    //      - can have a public variable to have the GGM get the variables and store them here.
    #region Player Variables
    public void SetMaxVelocity(float setMaxVelocity) 
    {
        playerData.maxVelocity = setMaxVelocity;
    }
    public float GetMaxVelocity() 
    {
        return playerData.maxVelocity;
    }
    public void SetMinVelocity(float setMinVelocity) 
    {
        playerData.minVelocity = setMinVelocity;
    }
    public float GetMinVelocity() 
    {
        return playerData.minVelocity;
    }
    public void SetMaxSpeedBonus(float setMaxSpeedBonus) 
    {
        playerData.maxSpeedBonus = setMaxSpeedBonus;
    }
    public float GetMaxSpeedBonus() 
    {
        return playerData.maxSpeedBonus;
    }
    public void SetJumpHeight(float setJumpHeight) 
    {
        playerData.jumpHeight = setJumpHeight;
    }
    public float GetJumpHeight() 
    {
        return playerData.jumpHeight;
    }
    public void SetAcceleration(int setAcceleration) 
    {
        playerData.acceleration = setAcceleration;
    }
    public int GetAcceleration() 
    {
        return playerData.acceleration;
    }
    public void SetScoreMarks(float[] setScoreMarks) 
    {
        // If all marks are set to 0, then there aren't 3 arguments being passed in for the level data
        
    }
    public float[] GetScoreMarks() 
    {
        if (scoreMarks.Length == 3)
        {
            return scoreMarks;
        }
        else
        {
            scoreMarks = new float[3] { 0.0f, 0.0f, 0.0f };
            return scoreMarks;
        }
    }
    public int GetScoreCap() {
        return scoreCap;
    }
    public void SetFreeMovement(bool setFreeMovement) {
        playerData.freeMovement = setFreeMovement;
    }
    public bool GetFreeMovement() {
        return playerData.freeMovement;
    }
    public GameObject GetPlayerBodySkin()
    {
        return playerData.skinBody;
    }
    public void SetPlayerBodySkin(GameObject skinBody)
    {
        playerData.skinBody = skinBody;
    }
    public GameObject GetPlayerFeetSkin()
    {
        return playerData.skinFeet;
    }
    public void SetPlayerFeetSkin(GameObject skinFeet)
    {
        playerData.skinFeet = skinFeet;
    }
    public GameObject GetPlayerHandsSkin()
    {
        return playerData.skinHands;
    }
    public void SetPlayerHandsSkin(GameObject skinHands)
    {
        playerData.skinHands = skinHands;
    }
    public GameObject GetPlayerHeadSkin()
    {
        return playerData.skinHead;
    }
    public void SetPlayerHeadSkin(GameObject skinHead)
    {
        playerData.skinHead = skinHead;
    }
    #endregion Player Variables

    #region GlobalMethods
    public void UseNotification(string description, float duration)
    {
        notification.UseNotification(description, duration);
    }
    public void UseNotification(string description, float duration, int type)
    {
        notification.UseNotification(description, duration, type);
    }
    #endregion GlobalMethods

    // TODO: will need to get the scenery prefab lists, enemy prefab lists
    // 			and will need to use an index with a data table to determine what is used per level
    void Awake ()
    {
        // Used to create a singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        // There will only be 1 instance of this object, and it transfers to other scenes
        DontDestroyOnLoad(gameObject);
        notification = GetComponentInChildren<Notification>();
        levelData = GetComponentInChildren<LevelData>();
        playerData = GetComponentInChildren<PlayerData>();
        spellData = GetComponentInChildren<SpellData>();
        completion = transform.GetChild(1).transform.GetChild(0).GetComponent<RewardGroup>();
        firstTime = transform.GetChild(1).transform.GetChild(1).GetComponent<RewardGroup>();
        mastery = transform.GetChild(1).transform.GetChild(2).GetComponent<RewardGroup>();
        // ----------------- Test Code ---------------------- //
        // -------------- End Test Code --------------------- //
	}

}
