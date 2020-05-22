using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class OWLevelController : MonoBehaviour {

#pragma warning disable 0649
    [SerializeField] private OWUIController owuic;
#pragma warning restore 0649
    private List<LevelData> levels;
    private GameObject hit;
    private GameObject levelsParent;
    private GlobalGameManager ggm;
    private LevelData levelData;
    private OWSkinController owsc;
    private RewardGroup completion;
    private RewardGroup firstTime;
    private RewardGroup mastery;
    private PlayFabData pfd;

    // TODO: Get completed level Performance Data from PlayFab and update to each level.

    void Awake ()
    {
        ggm = GlobalGameManager.instance;
        levels = new List<LevelData>();
        pfd = GameObject.Find("GlobalGameManager/PlayFabController/PlayFabDataController").GetComponent<PlayFabData>();
	}

    private void Start()
    {
        Input.multiTouchEnabled = true;
        levelsParent = GameObject.Find("LevelDesign/Levels");
        owsc = GameObject.Find("Data/ClientData").GetComponent<OWSkinController>();
        for (int i = 0; i < levelsParent.transform.childCount; i++)
        {
            levels.Add(levelsParent.transform.GetChild(i).transform.GetChild(0).GetComponent<LevelData>());
            // TODO: Populate level Completion Data from Playfab for each child.
        }

        StartCoroutine(PopulateCompletedLevels());
        
        // TODO: Fade out splash screen
    }

    private void Update ()
    {
		// ----------------- Test Code ---------------------- //

		if (Input.GetMouseButtonDown(0)){
            // Check they raycast
			if (!EventSystem.current.IsPointerOverGameObject()){
				CheckRayCast (Input.mousePosition);
			}
			
			// If legit, get data
			// If Legit, popluate data
			// Bring up UI window with populated data & info
		}
        // --------------- End Test Code --------------------- //

        if (Input.touchCount > 0 && !ggm.mobileBuild)
        {
            ggm.mobileBuild = true;
        }
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			// Checks if the touch was hitting a UI element
			if (!EventSystem.current.IsPointerOverGameObject ()) {
				CheckRayCast (Input.GetTouch (0).position);
			}
		}
    }

	private void CheckRayCast (Vector2 screenPos)
    {
		Ray ray = Camera.main.ScreenPointToRay (screenPos);
		RaycastHit hitInfo;
        if (Physics.Raycast (ray, out hitInfo, 100.0f))
        {
            hit = hitInfo.collider.gameObject.transform.parent.gameObject;
            if (hit.tag == "Level")
            {
                
                levelData = hit.transform.GetChild(0).GetComponent<LevelData>();
                completion = hit.transform.GetChild(0).transform.GetChild(0).GetComponent<RewardGroup>();
                firstTime = hit.transform.GetChild(0).transform.GetChild(1).GetComponent<RewardGroup>();
                mastery = hit.transform.GetChild(0).transform.GetChild(2).GetComponent<RewardGroup>();
                owuic.OnLevelClick(levelData);
                // Get data from PlayFab
                StartCoroutine(PopulateLevelData(levelData));

                // TODO: Set up Loading spinning Icon
                // Get data from PlayerPrefs
                // Set all data
                // TODO: set all the LevelData to the InfoPanel
                return;
            }
		}
        owuic.CloseLevelInfoPanel();
	}

	public void StartLevel ()
    {
        //TODO: Get the level statistics and set them to the global game manager
        SetGGMLevelData();
        // TODO: Change it to a case statement regarding perp & free motion
        SceneManager.LoadScene("RaceLevelPerpMotion");
	}

    private void SetGGMLevelData()
    {
        // TODO: Make sure none of them are null, and what to do if the List.count is 0
        // TODO: Set the Active Staff from ClientData
        // TODO: Animation for loading screen
        List<GameObject> tempList;
        int index;
        tempList = owsc.GetAvailableBodySkins();
        index = PlayerPrefs.GetInt("SkinBody");
        ggm.SetPlayerBodySkin(tempList[index]);
        tempList = owsc.GetAvailableFeetSkins();
        index = PlayerPrefs.GetInt("SkinFeet");
        ggm.SetPlayerFeetSkin(tempList[index]);
        tempList = owsc.GetAvailableHandSkins();
        index = PlayerPrefs.GetInt("SkinHands");
        ggm.SetPlayerHandsSkin(tempList[index]);
        tempList = owsc.GetAvailableHeadSkins();
        index = PlayerPrefs.GetInt("SkinHead");
        ggm.SetPlayerHeadSkin(tempList[index]);
        ggm.SetLevelData(levelData);
        ggm.SetCompletionRewards(completion.size, completion.color, completion.quantity);
        ggm.SetFirstTimeRewards(firstTime.size, firstTime.color, firstTime.quantity);
        ggm.SetMasteryRewards(mastery.size, mastery.color, mastery.quantity);
        // TODO: Wait for Playfab to return true from Populate level Data, then load level.

    }

    private IEnumerator PopulateCompletedLevels()
    {
        int activeLevels;
        pfd.OnGetLevelsCompleted();
        yield return StartCoroutine(WaitForPreFab(15.0f));

        if (pfd.GetRequestSuccess())
        {
            activeLevels = pfd.GetCompletedLevelCount() + 1;
        }
        else
        {
            activeLevels = 1;
        }
        int levelsCount = levelsParent.transform.childCount;
        if (activeLevels > levelsCount)
        {
            activeLevels = levelsCount;
        }
        for (int i = 0; i < activeLevels; i++)
        {
            levelsParent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private IEnumerator PopulateLevelData(LevelData ld)
    {
        pfd.OnGetCompletedLevelData(ld.GetLevelName());
        bool returnSuccess;
        float time = 0;
        while (!pfd.GetRequestReturned())
        {
            yield return new WaitForSeconds(0.1f);
            time += Time.deltaTime;
            if (time > 15.0f)
                break;
            else { }
                // TODO: Time Out
        }
        returnSuccess = pfd.GetRequestSuccess();
        if (returnSuccess)
        {
            string lp = pfd.GetLevelCompletionData();
            ggm.SetLevelPerformance(lp);
            // TODO: Set data to the proper UI stuffs.
        }
        else
        {
            // TODO: Set default values.
        }
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
}
