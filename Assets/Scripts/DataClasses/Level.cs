using UnityEngine;

public struct Level
{
    [Range(0, 500)]
    public int obstacleDifficulty;
    [Range(0, 500)]
    public int enemyDifficulty;
    public int levelSize;
    public int terrainObjectsPerTile;
    public int scoreCap;
    public float[] scoreMarks;
    public string seed;

    public float gravity;
    public bool useRandomSeed;

    public int[] firstTimeR;
    public int[] completionR;
    public int[] masteryR;
}