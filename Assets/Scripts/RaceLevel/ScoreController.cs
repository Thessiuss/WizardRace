using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreController : MonoBehaviour {

    public static bool isCasting;

#pragma warning disable 0649
    [SerializeField] private GameObject player;
#pragma warning restore 0649

    private Rigidbody playerRB;
    private GlobalGameManager ggm;
    private LevelController lc;
    private PlayFabData pfd;
    private RewardGroup completionRewards;
    private RewardGroup firstTimeRewards;
    private RewardGroup masteryRewards;
    private RLUIController rluiController;
    private Dictionary<string, int> endRewards;
    private bool [] rgbCrystals = new bool [3];
    private bool freeMovement;
    private float levelLength;
    private float spellMultiplier;
    private float spellMultiplierCap;
    private float spellScoreMultiplier = 1;
    private float speedMultiplier = 1.0f;
    private float speedMultiplierCap;
    private float maxSpeed;
    private float minSpeed;
    private float cumulativeVelocity;
    private float lastPos;      // Used to determine score relative to position;
    private float scoreValue;
    private float scoreUnmitigated;
    private float [] scoreMarks = new float [3] { 0, 0, 0 };
    private float spellTimeMax = 0;
    private float spellTime = 0;
    private float levelTime;
    private int wispCounter;
    private int wispTotal;
    private int scoreCap;
    private string levelPerformance = string.Empty;
    private string pfLevelData = string.Empty;
    private string levelName;

    // TODO: turn off the text if there's no multiplier
    // TODO: turn on the text if there is a mutliplier

    void Awake() {
        scoreValue = 0;
        wispCounter = 0;
        levelTime = 0;
        playerRB = player.GetComponent<Rigidbody>();
        endRewards = new Dictionary<string, int>() {
            {"SmallBlue", 0 },
            {"SmallGreen", 0 },
            {"SmallPurple", 0 },
            {"SmallRed", 0 },
            {"SmallYellow", 0 },
            {"MediumBlue", 0 },
            {"MediumGreen", 0 },
            {"MediumPurple", 0 },
            {"MediumRed", 0 },
            {"MediumYellow", 0 },
            {"LargeBlue", 0 },
            {"LargeGreen", 0 },
            {"LargePurple", 0 },
            {"LargeRed", 0 },
            {"LargeYellow", 0 },
            {"HugeBlue", 0 },
            {"HugeGreen", 0 },
            {"HugePurple", 0 },
            {"HugeRed", 0 },
            {"HugeYellow", 0}
        };
    }

    // TODO: Check which crystals are already collected and populate accordingly.
    void Start() {
        ggm = GlobalGameManager.instance;
        lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        rluiController = GameObject.Find("UI").GetComponent<RLUIController>();
        pfd = GameObject.Find("GlobalGameManager/PlayFabController/PlayFabDataController").GetComponent<PlayFabData>();
        completionRewards = GameObject.Find("GlobalGameManager/LevelData/Completion").GetComponent<RewardGroup>();
        firstTimeRewards = GameObject.Find("GlobalGameManager/LevelData/FirstTime").GetComponent<RewardGroup>();
        masteryRewards = GameObject.Find("GlobalGameManager/LevelData/Mastery").GetComponent<RewardGroup>();
        levelLength = ggm.GetLevelLength();
        spellMultiplier = ggm.GetSpellMultiplier();
        spellMultiplierCap = ggm.GetSpellMultiplierCap();
        speedMultiplierCap = ggm.GetMaxSpeedBonus();
        maxSpeed = ggm.GetMaxVelocity();
        scoreMarks = ggm.GetScoreMarks();
        scoreCap = ggm.GetScoreCap();
        freeMovement = ggm.GetFreeMovement();
        levelName = ggm.GetLevelName();
        if (freeMovement) levelTime = 0;
        levelPerformance = ggm.GetLevelPerformance();
        if (levelPerformance == null)
            levelPerformance = string.Empty;
        if (levelPerformance.Contains("R"))
        {
            rluiController.OnCrystal("Red");
            lc.CollectRedCrystal();
            OnCrystalCollect("Red");
        }
        if (levelPerformance.Contains("G"))
        {
            rluiController.OnCrystal("Green");
            lc.CollectGreenCrystal();
            OnCrystalCollect("Green");
        }
        if (levelPerformance.Contains("B"))
        {
            rluiController.OnCrystal("Blue");
            lc.CollectBlueCrystal();
            OnCrystalCollect("Blue");
        }
        StartCoroutine(GetCompletionData());
    }

    void Update() {
        if (freeMovement && LevelController.isActive) {
            AdjustTime();
        }
        if (LevelController.isActive)
        {
            cumulativeVelocity += playerRB.velocity.x * Time.deltaTime;
            if (SpellController.isCasting)
            {
                spellTime += Time.deltaTime;
                if (spellTime > spellTimeMax)
                {
                    spellTimeMax = spellTime;
                }
            }
            else
            {
                spellTime = 0;
            }
        }
        AdjustSpellMultiplier();
        AdjustSpeedMultiplier();
        AdjustTerrainProgress();
        AdjustScore();
    }

    public void OnCrystalCollect(string crystalColor)
    {
        switch (crystalColor)
        {
            case "Red":
                rgbCrystals[0] = true;
                levelPerformance += "R";
                break;
            case "Green":
                rgbCrystals [1] = true;
                levelPerformance += "G";
                break;
            case "Blue":
                rgbCrystals [2] = true;
                levelPerformance += "B";
                break;
        }
        rluiController.OnCrystal(crystalColor);
    }
    // Need to keep track of time spend in the level if it's not perpetual motion
    private void AdjustTime() {
        levelTime += Time.deltaTime;
    }

    private void AdjustSpellMultiplier() {
        if (isCasting && (spellScoreMultiplier <= spellMultiplierCap)) {
            spellScoreMultiplier += spellMultiplier * Time.deltaTime;
        }
        if (!isCasting && spellScoreMultiplier > 1.0f) {
            spellScoreMultiplier = 1.0f;
        }
        rluiController.SpellSlider( (spellScoreMultiplier - 1) / (spellMultiplierCap - 1));
    }

    //TODO: adjust the speedmultiplier to be affected by the upgrades. Divided by 10 or something.
    private void AdjustSpeedMultiplier() {
        float currentSpeed = playerRB.velocity.x;
        float speedRatio = Mathf.Abs(currentSpeed / maxSpeed);

        if (speedRatio >= 0) {
            speedMultiplier = 1 + speedRatio * (speedMultiplierCap - 1);
        }
        else {
            speedMultiplier = 1;
        }
        rluiController.SpeedSlider(speedRatio);
    }

    private void AdjustTerrainProgress()
    {
        float distanceRatio;
        distanceRatio = Mathf.Abs(player.transform.position.x / levelLength);
        rluiController.DistanceSlider(distanceRatio);
    }

    private void AdjustScore()
    {
        scoreValue += (player.transform.position.x - lastPos) * speedMultiplier * spellScoreMultiplier;
        lastPos = player.transform.position.x;
    }

    public void AddWisp(int pointValue)
    {
        wispCounter += pointValue;
        rluiController.SetWispCounter(wispCounter);
    }

    public void AddWispTotal(int value)
    {
        wispTotal += value;
    }

    public void AddPointValue(int value)
    {
        scoreUnmitigated += value;
    }

    // Triggered by LevelController EndLevel
    public void EndLevelCalculations()
    {
        scoreValue = (scoreValue * 0.1f * levelLength / levelTime) + scoreUnmitigated;
        float scoreRatio = scoreValue / scoreCap;
        // TODO: All of this needs to move to later.
        
        StartCoroutine(EndLevelPlayFab(scoreRatio));
    }

    private void GiveRewards(RewardGroup rewards)
    {
        string key = string.Empty;
        for (int i = 0; i < rewards.quantity.Count; i++)
        {
            if (rewards.size [i] == "Kin")
            {
                // TODO: set up kin reward here
                continue;
            }
            key = rewards.size [i] + rewards.color [i];
            endRewards [key] += rewards.quantity [i];
        }
    }

    private string CombineLevelData(string pfLevelData)
    {
        string combinedData = string.Empty;
        if (pfLevelData.Contains("M") || levelPerformance.Contains("M"))
        {
            combinedData += "M";
        }
        if (pfLevelData.Contains("F") || levelPerformance.Contains("F"))
        {
            combinedData += "F";
        }
        if (pfLevelData.Contains("R") || levelPerformance.Contains("R"))
        {
            combinedData += "R";
        }
        if (pfLevelData.Contains("G") || levelPerformance.Contains("G"))
        {
            combinedData += "G";
        }
        if (pfLevelData.Contains("B") || levelPerformance.Contains("B"))
        {
            combinedData += "B";
        }
        return combinedData;
    }
    
    #region IEnumerators
    private IEnumerator EndLevelPlayFab(float scoreRatio)
    {
        // Give Rewards according to LevelData
        if (scoreRatio >= scoreMarks [2] * 0.01f && !pfLevelData.Contains("M"))
        {
            levelPerformance += "M";
            rluiController.OnRewardsMastery();
            GiveRewards(masteryRewards);
        }
        bool ft = false;
        if (!pfLevelData.Contains("F")) // TODO: Hasn't been given First Time
        {
            levelPerformance += "F";

            rluiController.OnRewardsFirstTime();
            GiveRewards(firstTimeRewards);
            ft = true;                  // Need to populate rewards with First Time, but can't quite update Prefab yet.
        }
        GiveRewards(completionRewards);                 // If they get to the end, they get at least completion.
        string combinedLevelData = CombineLevelData(pfLevelData);
        rluiController.EndLevel(levelTime, scoreValue, scoreRatio, wispCounter, 30, cumulativeVelocity / levelTime, spellTimeMax);

        if (ft)
        {
            yield return StartCoroutine(IncrementLevelsCompleted());
        }
        
        pfd.OnUpdateLevelCompletionData(levelName, combinedLevelData);
        yield return StartCoroutine(WaitForPreFab(15.0f));

        pfd.OnUpdateCrystalCount(endRewards);
        yield return StartCoroutine(WaitForPreFab(15.0f));

        //TODO: Need to make sure all of the code runs before they can "continue" back to the Overworld
        // after last one fires, set a boolean "check" to load levels.
    }

    private IEnumerator GetCompletionData()
    {
        // pfd.IncrementCompletedLevel();
        // Get level data, then see what rewards to give (compare completed data to performant data, then update rewards
        pfd.OnGetCompletedLevelData(levelName);
        yield return StartCoroutine(WaitForPreFab(15.0f));
        if (pfd.GetRequestSuccess())
        {
            pfLevelData = pfd.GetLevelCompletionData();
            if (pfLevelData == null)
                pfLevelData = string.Empty;
        }
        else
        {
            // Time out
        }
    }

    private IEnumerator IncrementLevelsCompleted()
    {
        int completedLevels = 0;
        pfd.OnGetLevelsCompleted();
        yield return StartCoroutine(WaitForPreFab(15.0f));
        if (pfd.GetRequestSuccess())
        {
            completedLevels = pfd.GetCompletedLevelCount();
        }
        else
            yield break;
        pfd.OnSetLevelsCompleted(completedLevels + 1);
        yield return StartCoroutine(WaitForPreFab(15.0f));
    }

    private IEnumerator WaitForPreFab(float duration)
    {
        float t = 0;
        bool checkReturn = false;
        while (!checkReturn)
        {
            t += Time.deltaTime;
            checkReturn = pfd.GetRequestReturned();
            yield return new WaitForSeconds(0.1f);
            if (t > duration)
            {
                ggm.UseNotification("Lost Connection", 2.0f);
                break;
            }
        }
    }
    #endregion IEnumerators
}
