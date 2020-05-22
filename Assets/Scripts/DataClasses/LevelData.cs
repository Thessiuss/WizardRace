using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    // Whenever changing values here, make sure to update the spreadsheet in Drive FIRST
    // Drive > Plans & Pipelines > Wizard Race > Task Creation
    // Please add 
#pragma warning disable 0649
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
    [Range(0, 100)]
    [SerializeField] private int enemyFrequency;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<int> enemySpawnOrder;
    [SerializeField] private List<GameObject> enemySpecificPrefabs;
    [SerializeField] private List<Vector3> enemySpawnPositions;
    [Range(0, 100)]
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
#pragma warning restore 0649

    public string GetLevelName()
    {
        return levelName;
    }
    public string GetLevelPerformance()
    {
        return levelPerformance;
    }
    public int GetInitialDelay()
    {
        return initialDelay;
    }
    public float GetRenderDistance()
    {
        return renderDistance;
    }
    public float GetGravity()
    {
        return gravity;
    }
    public float GetFallMult()
    {
        return fallMult;
    }
    public float GetLowJumpMult()
    {
        return lowJumpMult;
    }
    public bool GetUseRandomSeed()
    {
        return useRandomSeed;
    }
    public string GetSeed()
    {
        return seed;
    }
    public int GetScoreCap()
    {
        return scoreCap;
    }
    public float[] GetScoreMarks()
    {
        return scoreMarks;
    }
    public int GetLevelLength()
    {
        return levelLength;
    }
    public List<Vector2> GetCrystalLocations()
    {
        return crystalLocations;
    }
    public int GetEnemyFrequency()
    {
        return enemyFrequency;
    }
    public List<GameObject> GetEnemyPrefabs()
    {
        return enemyPrefabs;
    }
    public List<int> GetEnemySpawnOrder()
    {
        return enemySpawnOrder;
    }
    public List<Vector3> GetEnemySpawnPositions()
    {
        return enemySpawnPositions;
    }
    public List<GameObject> GetEnemySpecificPrefabs()
    {
        return enemySpecificPrefabs;
    }
    public int GetObstacleFrequency()
    {
        return obstacleFrequency;
    }
    public List<GameObject> GetObstaclePrefabs()
    {
        return obstaclePrefabs;
    }
    public List<int> GetObstacleSpawnOrder()
    {
        return obstacleSpawnOrder;
    }
    public List<Vector3> GetObstacleSpawnPositions()
    {
        return obstacleSpawnPositions;
    }
    public List<GameObject> GetSpecialObjectPrefabs()
    {
        return specialObjectPrefabs;
    }
    public List<Vector3> GetSpecialObjectLocations()
    {
        return specialObjectLocations;
    }
    public List<GameObject> GetTerrainPrefabs()
    {
        return terrainPrefabs;
    }
    public List<int> GetTerrainSpawnOrder()
    {
        return terrainSpawnOrder;
    }
    public List<GameObject> GetWispPrefabs()
    {
        return wispPrefabs;
    }
    public List<GameObject> GetWispClusters()
    {
        return wispClusters;
    }
    public List<Vector2> GetWispPositions()
    {
        return wispPositions;
    }
    public List<int> GetWispSpawnOrder()
    {
        return wispSpawnOrder;
    }

}
