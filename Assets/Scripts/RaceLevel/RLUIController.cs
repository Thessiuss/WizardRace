using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RLUIController : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject endLevelPanel;
    public GameObject lootCardPrefab;
    public GameObject headerPrefab;
    public GameObject listSpacerPrefab;

    // EP: End Panel
    // EPR: End Panel Record
#pragma warning disable 0649
    [SerializeField] private GameObject bestRecordEP;
    [SerializeField] private GameObject blueCrystalEP;
    [SerializeField] private GameObject blueCrystalAuraEP;
    [SerializeField] private GameObject greenCrystalEP;
    [SerializeField] private GameObject greenCrystalAuraEP;
    [SerializeField] private GameObject newRecordSpeed;
    [SerializeField] private GameObject newRecordSpell;
    [SerializeField] private GameObject newRecordTime;
    [SerializeField] private GameObject newRecordWisp;
    [SerializeField] private GameObject performanceEP;
    [SerializeField] private GameObject redCrystalEP;
    [SerializeField] private GameObject redCrystalAuraEP;
    [SerializeField] private GameObject rewardContentC;
    [SerializeField] private GameObject rewardContentFT;
    [SerializeField] private GameObject rewardContentM;
    [SerializeField] private GameObject rewardGroupM;
    [SerializeField] private GameObject rewardGroupFT;
    [SerializeField] private GameObject rewardsEP;
    [SerializeField] private GameObject scoreParIn;                 //Score Particle Indicator
    [SerializeField] private GameObject sparkliesEP;                //Score Value Sparkly particle system
    [SerializeField] private GameObject spdParIn;                   //Speed Particle Indicator
    [SerializeField] private GameObject splParIn;                   //Spell Particle Indicator
    [SerializeField] private GameObject terParIn;                   //Terrain Particle Indicator
    [SerializeField] private Image blueCrystal;
    [SerializeField] private Image greenCrystal;
    [SerializeField] private Image redCrystal;
    [SerializeField] private Image speedSlider;
    [SerializeField] private Image spellSlider;
    [SerializeField] private Image scoreSliderEP;
    [SerializeField] private Image terrainProgress;
    [SerializeField] private Text wispCounter;
    [SerializeField] private TextMeshProUGUI levelNameEP;
    [SerializeField] private TextMeshProUGUI scoreEP;
    [SerializeField] private TextMeshProUGUI scoreEPR;
    [SerializeField] private TextMeshProUGUI speedEP;
    [SerializeField] private TextMeshProUGUI speedEPR;
    [SerializeField] private TextMeshProUGUI spellEP;
    [SerializeField] private TextMeshProUGUI spellEPR;
    [SerializeField] private TextMeshProUGUI timeEP;
    [SerializeField] private TextMeshProUGUI timeEPR;
    [SerializeField] private TextMeshProUGUI wispCounterEP;
    [SerializeField] private TextMeshProUGUI wispCounterEPR;
    [SerializeField] private TextMeshProUGUI wispTotalEP;
    [SerializeField] private TextMeshProUGUI wispTotalEPR;
#pragma warning restore 0649

    private GlobalGameManager ggm;
    private RectTransform spdParInRct;
    private RectTransform splParInRct;
    private RectTransform terParInRct;
    private ResourcesData resourcesData;
    private RewardGroup completionRewards;
    private RewardGroup firstTimeRewards;
    private RewardGroup masteryRewards;
    private float levelTimeScale= 1;
    private float [] scoreMarks;
    private string levelName;


    private void Awake()
    {
        wispCounter.text = "0";
        spdParInRct = spdParIn.GetComponent<RectTransform>();
        splParInRct = splParIn.GetComponent<RectTransform>();
        terParInRct = terParIn.GetComponent<RectTransform>();
    }

    void Start () {
        ggm = GlobalGameManager.instance;
        resourcesData = GameObject.Find("GlobalGameManager/ResourcesData").GetComponent<ResourcesData>();
        completionRewards = GameObject.Find("GlobalGameManager/LevelData/Completion").GetComponent<RewardGroup>();
        firstTimeRewards = GameObject.Find("GlobalGameManager/LevelData/FirstTime").GetComponent<RewardGroup>();
        masteryRewards = GameObject.Find("GlobalGameManager/LevelData/Mastery").GetComponent<RewardGroup>();
        scoreMarks = ggm.GetScoreMarks();
        levelName = ggm.GetLevelName();
        PopulateRewards(completionRewards, rewardContentC, "Completion", 1);
        PopulateRewards(firstTimeRewards, rewardContentFT, "First Time", 1);
        PopulateRewards(masteryRewards, rewardContentM, "Mastery", 1);
    }

    #region PausePanel
    // Will freeze time in the game so nothing changes
    public void PauseButton ()
    {
		pausePanel.gameObject.SetActive (true);
		pauseButton.gameObject.SetActive (false);
        levelTimeScale = Time.timeScale;
        ParticleToggle(false);
        Time.timeScale = 0;
	}
		
	public void PauseResume ()
    {
		pausePanel.gameObject.SetActive (false);
		pauseButton.gameObject.SetActive (true);
        Time.timeScale = levelTimeScale;
        ParticleToggle(true);
    }

    public void ExitButton ()
    {
        Time.timeScale = 1;
		SceneManager.LoadScene ("OverWorld");
	}
		
	public void ExitButtonConfirm ()
    {
		
	}

	public void SettingsButton ()
    {
		
	}

	public void RestartButton ()
    {
        Time.timeScale = 1;
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	public void RestartButtonConfirm ()
    {
	    
	}
    #endregion PausePanel

    #region Crystals
    public void OnCrystal(string crystal)
    {
        switch (crystal)
        {
            case "Red":
                redCrystal.color = new Color32(255, 255, 255, 255);
                redCrystalEP.SetActive(true);
                redCrystalAuraEP.SetActive(true);
                break;
            case "Green":
                greenCrystal.color = new Color32(255, 255, 255, 255);
                greenCrystalEP.SetActive(true);
                greenCrystalAuraEP.SetActive(true);
                break;
            case "Blue":
                blueCrystal.color = new Color32(255, 255, 255, 255);
                blueCrystalEP.SetActive(true);
                blueCrystalAuraEP.SetActive(true);
                break;
        }
    }
    #endregion Crystals

    #region Update
    // Called each frame
    public void SpeedSlider(float ratio)
    {
        spdParInRct.localPosition = new Vector3((374 * ratio) - 374, spdParIn.transform.localPosition.y, 0.0f); 
        speedSlider.fillAmount = ratio;
    }
    public void SpellSlider(float ratio)
    {
        splParInRct.localPosition = new Vector3((-374 * ratio), splParIn.transform.localPosition.y, 0.0f);
        spellSlider.fillAmount = ratio;
    }
    public void DistanceSlider(float ratio)
    {
        terParInRct.localPosition = new Vector3((792 * ratio) - 792, terParIn.transform.localPosition.y, 0.0f);
        terrainProgress.fillAmount = ratio;
    }
    #endregion Update

    #region EndLevel
    // Called from ScoreController EndLevelCalculations
    public void EndLevel(float time, float score, float scoreRatio, int wispCatch, int wispTotal, float avgSpeed, float avgSpell)
    {
        ParticleToggle(false);
        levelNameEP.text = levelName;
        timeEP.text = "" + Math.Round(time, 2);
        wispCounterEP.SetText("" + wispCatch);
        wispTotalEP.SetText(" /" + wispTotal);
        wispTotalEPR.SetText(" /" + wispTotal);
        speedEP.SetText("" + Math.Round(avgSpeed,2));
        spellEP.SetText("" + Math.Round(avgSpell,2));

        bool hasPerf = PlayerPrefs.HasKey(levelName + "time");
        if (hasPerf)
        {
            float prefsF = 0;
            int prefsI = 0;
            prefsF = PlayerPrefs.GetFloat(levelName + "time");
            if (time < prefsF)
            {
                PlayerPrefs.SetFloat(levelName + "time", time);
                newRecordTime.SetActive(true);
                timeEPR.text = "" + Math.Round(time, 2);
            }
            else
            {
                timeEPR.text = Math.Round(prefsF, 2) + "";
            }
            prefsF = PlayerPrefs.GetFloat(levelName + "score");
            if (score > prefsF)
            {
                PlayerPrefs.SetFloat(levelName + "score", score);
                scoreEPR.text = "" + Math.Round(score, 2);
            }
            else
            {
                scoreEPR.text = "" + Math.Round(prefsF, 1);
            }
            prefsI = PlayerPrefs.GetInt(levelName + "wispCatch");
            if (wispCatch > prefsI)
            {
                PlayerPrefs.SetInt(levelName + "wispCatch", wispCatch);
                newRecordWisp.SetActive(true);
                wispCounterEPR.SetText("" + wispCatch);
            }
            else
            {
                wispCounterEPR.SetText("" + prefsI);
            }
            prefsF = PlayerPrefs.GetFloat(levelName + "avgSpeed");
            if (avgSpeed > prefsF)
            {
                newRecordSpeed.SetActive(true);
                PlayerPrefs.SetFloat(levelName + "avgSpeed", avgSpeed);
                speedEPR.text = "" + Math.Round(avgSpeed, 2);
            }
            else
            {
                double speed = Math.Round(prefsF, 2);
                speedEPR.text = "" + speed;
            }
            prefsF = PlayerPrefs.GetFloat(levelName + "avgSpell");
            if (avgSpell > prefsF)
            {
                PlayerPrefs.SetFloat(levelName + "avgSpell", avgSpell);
                newRecordSpell.SetActive(true);
                spellEPR.text = "" + Math.Round(avgSpell, 2);
            }
            else
            {
                spellEPR.text = Math.Round(prefsF, 2) + "";
            }
        }
        else
        {
            PlayerPrefs.SetFloat(levelName + "time", time);
            timeEPR.text = "" + Math.Round(time, 2);
            PlayerPrefs.SetFloat(levelName + "score", score);
            scoreEPR.text = "" + Math.Round(score, 2);
            PlayerPrefs.SetInt(levelName + "wispCatch", wispCatch);
            wispCounterEPR.SetText("" + wispCatch);
            PlayerPrefs.SetFloat(levelName + "avgSpeed", avgSpeed);
            speedEPR.text = "" + Math.Round(avgSpeed, 2);
            PlayerPrefs.SetFloat(levelName + "avgSpell", avgSpell);
            spellEPR.text = "" + Math.Round(avgSpell, 2);
        }
        
        // Adjust the rest of the marks on the slider and the score color.
        scoreSliderEP.fillAmount = scoreRatio;
        if (scoreRatio > 0.3f && scoreRatio < 0.7f)
        {
            var main = scoreParIn.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            main.startColor = new Color(1.0f, 1.0f, 1.0f, 0.14f);
            main = scoreParIn.transform.GetChild(1).GetComponent<ParticleSystem>().main;
            main.startColor = new Color(1.0f, 1.0f, 1.0f, 0.14f);
        }
        if (scoreRatio > 1.0f)
        {
            scoreRatio = 1.0f;
        }
        scoreParIn.transform.localPosition = new Vector3((680 * scoreRatio) - 340, scoreParIn.transform.localPosition.y, 0.0f);
        if (scoreRatio >= scoreMarks [2] * 0.01f)
        {
            scoreEP.color = new Color32(134, 250, 248, 255);
            sparkliesEP.SetActive(true);
        }
        else if (scoreRatio >= scoreMarks [1] * 0.01f)
        {
            scoreEP.color = new Color32(59, 234, 70, 255);
        }
        else if (scoreRatio >= scoreMarks [0] * 0.01f)
        {
            scoreEP.color = new Color32(255, 188, 64, 255);
        }
        else
        {
            scoreEP.color = new Color32(255,69,64,255);
        }
        if (score <= 0)
        {
            scoreEP.text = "ZZZ zzz ...";
        }
        scoreEP.text = "" + Math.Round(score, 2);
        endLevelPanel.gameObject.SetActive(true);
    }

    public void OnPerformanceBand()
    {
        performanceEP.transform.SetSiblingIndex(3);
    }
    public void OnRecordBand()
    {
        bestRecordEP.transform.SetSiblingIndex(3);
    }
    public void OnRewardsBand()
    {
        rewardsEP.transform.SetSiblingIndex(3);
    }
    public void OnRewardsFirstTime()
    {
        rewardGroupFT.SetActive(true);
    }
    public void OnRewardsMastery()
    {
        rewardGroupM.SetActive(true);
    }

    private void PopulateRewards(RewardGroup rewardGroup, GameObject parent, string caption, int endSpaces)
    {
        
        for (int i = 0; i < rewardGroup.size.Count; i++)
        {
            // instantiate prefab
            GameObject lootCard = (GameObject)GameObject.Instantiate(lootCardPrefab, 
                lootCardPrefab.transform.position, lootCardPrefab.transform.rotation);
            // Get, compare, and set the picture according to which item it is
            Image cardImage = lootCard.transform.GetChild(0).GetComponent<Image>();
            int spriteIndex = GetSpriteIndex(rewardGroup.size[i], rewardGroup.color[i]);
            cardImage.sprite = resourcesData.rewardSprites[spriteIndex];
            lootCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = rewardGroup.size[i] + " " + rewardGroup.color[i];
            lootCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = rewardGroup.quantity[i] + " ";
            lootCard.transform.SetParent(parent.transform, false);
        }
        //Dummy object to take up space at the end of a reward group to add separation
        if (endSpaces > 0)
        {
            for (int i = 0; i < endSpaces; i++)
            {
                GameObject spacer = (GameObject)GameObject.Instantiate(listSpacerPrefab,
                 listSpacerPrefab.transform.position, listSpacerPrefab.transform.rotation);
                spacer.transform.SetParent(parent.transform, false);
            }
        }
    }

    private int GetSpriteIndex(string size, string color)
    {
        int index = 0;
        switch (color)
        {
            case "Kin":
                return index;
            case "Green":
                index += 1;
                break;
            case "Red":
                index += 2;
                break;
            case "Blue":
                index += 3;
                break;
            case "Yellow":
                index += 4;
                break;
            case "Purple":
                index += 5;
                break;
            default:
                index += 0;
                break;
        }
        switch (size)
        {
            case "Small":
                index += 0;
                break;
            case "Medium":
                index += 5;
                break;
            case "Large":
                index += 10;
                break;
            case "Huge":
                index += 15;
                break;
            default:
                index += 0;
                break;
        }
        return index;
    }
    #endregion EndLevel

    // TODO: fix this methodology to not be such trash
    public void SetWispCounter(int value)
    {
        wispCounter.text = value + "";
    }

    private void ParticleToggle(bool toggle)
    {
        terParIn.SetActive(toggle);
        splParIn.SetActive(toggle);
        spdParIn.SetActive(toggle);
    }
}
