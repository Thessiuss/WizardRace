using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class PlayFabData : MonoBehaviour
{
    public int experiencePoints;
    public int experience;
    public int playerHealth;
    public int jumpHeight;
    public int maxVelocity;
    public int minVelocity;
    public int speedBonus;
    public int acceleration;

    private Dictionary<string, int> rewardsEL;      // End Level rewards coming in from ScoreController EndLevelCalculations
    private Dictionary<string, int> crystalCount;   // Total inventory of crystals
    private Dictionary<string, string> crystalParseDict1;    // Used to change dictionary structure to save in Playfab UserData
    private Dictionary<string, string> crystalParseDict2;    // SetUserData can only have dictionaries of size 10. need to split
    private List<string> crystalRefKeys;
    
    private string levelCompletionData;
    private string levelName;
    private string userID;
    private int completedLevelCount = 0;
    private bool requestReturned;
    private bool requestSuccess;

    private void Awake()
    {
        levelName = string.Empty;
        levelCompletionData = string.Empty;
        crystalCount = new Dictionary<string, int>()
        {
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
        crystalParseDict1 = new Dictionary<string, string>()
        {
            {"SmallBlue", ""},
            {"SmallGreen", "" },
            {"SmallPurple", "" },
            {"SmallRed", "" },
            {"SmallYellow", "" },
            {"MediumBlue", "" },
            {"MediumGreen", "" },
            {"MediumPurple", "" },
            {"MediumRed", "" },
            {"MediumYellow", "" },
            
        };
        crystalParseDict2 = new Dictionary<string, string>()
        {
            {"LargeBlue", "" },
            {"LargeGreen", "" },
            {"LargePurple", "" },
            {"LargeRed", "" },
            {"LargeYellow", "" },
            {"HugeBlue", "" },
            {"HugeGreen", "" },
            {"HugePurple", "" },
            {"HugeRed", "" },
            {"HugeYellow", ""}
        };
        crystalRefKeys = new List<string>()
        {
            "SmallBlue",
            "SmallGreen",
            "SmallPurple",
            "SmallRed",
            "SmallYellow",
            "MediumBlue",
            "MediumGreen",
            "MediumPurple",
            "MediumRed",
            "MediumYellow",
            "LargeBlue",
            "LargeGreen",
            "LargePurple",
            "LargeRed",
            "LargeYellow",
            "HugeBlue",
            "HugeGreen",
            "HugePurple",
            "HugeRed",
            "HugeYellow"
        };
        
    }

    #region AccessorMethods
    public int GetCompletedLevelCount()
    {
        return completedLevelCount;
    }
    public Dictionary<string, int> GetCrystalCount()
    {
        return crystalCount;
    }
    public string GetLevelCompletionData()
    {
        return levelCompletionData;
    }
    public bool GetRequestReturned()
    {
        return requestReturned;
    }
    public bool GetRequestSuccess()
    {
        return requestSuccess;
    }    
    public void PlayFabResultPopulation(LoginResult result)
    {
        userID = result.PlayFabId;
    }
    
    
    #endregion AccessorMethods

    #region PlayerStats
    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "ExperiencePoints", Value = experiencePoints },
                new StatisticUpdate { StatisticName = "Experience", Value = experience },
                new StatisticUpdate { StatisticName = "PlayerHealth", Value = playerHealth },
                new StatisticUpdate { StatisticName = "JumpHeight", Value = jumpHeight },
                new StatisticUpdate { StatisticName = "MaxVelocity", Value = maxVelocity },
                new StatisticUpdate { StatisticName = "MinVelocity", Value = minVelocity },
                new StatisticUpdate { StatisticName = "SpeedBonus", Value = speedBonus },
                new StatisticUpdate { StatisticName = "Acceleration", Value = acceleration },
            }
        },
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    private void GetStats()
    {
        requestReturned = false;
        requestSuccess = false;
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    // LevelCompletion is a Cloud based PlayerStatistic
    public void IncrementCompletedLevel()
    {
        requestReturned = false;
        requestSuccess = false;
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest   
        {
            FunctionName = "IncrementCompletedLevel"
        },
        OnCloudUpdateScript,
        OnErrorShared
        );
    }

    private void OnGetStats(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            switch (eachStat.StatisticName)
            {
                case "ExperiencePoints":
                    experiencePoints = eachStat.Value;
                    break;
                case "Experience":
                    experience = eachStat.Value;
                    break;
                case "PlayerHealth":
                    playerHealth = eachStat.Value;
                    break;
                case "JumpHeight":
                    jumpHeight = eachStat.Value;
                    break;
                case "MaxVelocity":
                    maxVelocity = eachStat.Value;
                    break;
                case "MinVelocity":
                    minVelocity = eachStat.Value;
                    break;
                case "SpeedBonus":
                    minVelocity = eachStat.Value;
                    break;
                case "Acceleration":
                    minVelocity = eachStat.Value;
                    break;
            }
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
        }
    }

    // Build the request object and access the API
    private void StartCloudUpdatePlayerStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new
            {
                ExperiencePoints = experiencePoints,
                Experience = experience,
                PlayerHealth = playerHealth,
                JumpHeight = jumpHeight,
                MaxVelocity = maxVelocity,
                MinVelocity = minVelocity,
                SpeedBonus = speedBonus,
                Acceleration = acceleration
            }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, OnCloudUpdateScript, OnErrorShared);
    }
    #endregion PlayerStats

    #region CrystalHandlers
    // Calls the UserData from PlayFab
    public void GetUserCrystalData()
    {
        requestSuccess = false;
        requestReturned = false;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = userID,
            Keys = crystalRefKeys
        },
            UserDataCrystalCallback,
            OnErrorShared);
    }

    // Updates the Crystal Inventory from the server if it was successful
    private void UserDataCrystalCallback(GetUserDataResult result)
    {
        requestSuccess = true;
        requestReturned = true;
        // Converts from string,string to string,int dictionary
        if (result.Data.ContainsKey("SmallBlue"))
        {
            crystalCount ["SmallBlue"] = int.Parse(result.Data ["SmallBlue"].Value);
            crystalCount ["SmallGreen"] = int.Parse(result.Data ["SmallGreen"].Value);
            crystalCount ["SmallPurple"] = int.Parse(result.Data ["SmallPurple"].Value);
            crystalCount ["SmallRed"] = int.Parse(result.Data ["SmallRed"].Value);
            crystalCount ["SmallYellow"] = int.Parse(result.Data ["SmallYellow"].Value);
            crystalCount ["MediumBlue"] = int.Parse(result.Data ["MediumBlue"].Value);
            crystalCount ["MediumGreen"] = int.Parse(result.Data ["MediumGreen"].Value);
            crystalCount ["MediumPurple"] = int.Parse(result.Data ["MediumPurple"].Value);
            crystalCount ["MediumRed"] = int.Parse(result.Data ["MediumRed"].Value);
            crystalCount ["MediumYellow"] = int.Parse(result.Data ["MediumYellow"].Value);
            crystalCount ["LargeBlue"] = int.Parse(result.Data ["LargeBlue"].Value);
            crystalCount ["LargeGreen"] = int.Parse(result.Data ["LargeGreen"].Value);
            crystalCount ["LargePurple"] = int.Parse(result.Data ["LargePurple"].Value);
            crystalCount ["LargeRed"] = int.Parse(result.Data ["LargeRed"].Value);
            crystalCount ["LargeYellow"] = int.Parse(result.Data ["LargeYellow"].Value);
            crystalCount ["HugeBlue"] = int.Parse(result.Data ["HugeBlue"].Value);
            crystalCount ["HugeGreen"] = int.Parse(result.Data ["HugeGreen"].Value);
            crystalCount ["HugePurple"] = int.Parse(result.Data ["HugePurple"].Value);
            crystalCount ["HugeRed"] = int.Parse(result.Data ["HugeRed"].Value);
            crystalCount ["HugeYellow"] = int.Parse(result.Data ["HugeYellow"].Value);
        }
        else
        {
            // TODO: If this triggered, the keys have not been created yet. Or something else.
        }
    }

    public void OnUpdateCrystalCount(Dictionary<string, int> rewards)
    {
        rewardsEL = rewards;
        StartCoroutine(UpdateCrystalCount());
    }

    private IEnumerator UpdateCrystalCount()
    {
        GetUserCrystalData();
        float secondCounter = 0.0f;
        while (!requestReturned)
        {
            yield return new WaitForSeconds(0.1f);
            secondCounter += 0.1f;
            if (secondCounter > 15.0f)
            {
                GlobalGameManager.instance.UseNotification("Connection to Server timed out.", 3.0f);
                break;
            }
        }
        if (requestSuccess)
        {
            crystalCount ["SmallBlue"] += rewardsEL ["SmallBlue"];
            crystalCount ["SmallGreen"] += rewardsEL ["SmallGreen"];
            crystalCount ["SmallPurple"] += rewardsEL ["SmallPurple"];
            crystalCount ["SmallRed"] += rewardsEL ["SmallRed"];
            crystalCount ["SmallYellow"] += rewardsEL ["SmallYellow"];
            crystalCount ["MediumBlue"] += rewardsEL ["MediumBlue"];
            crystalCount ["MediumGreen"] += rewardsEL ["MediumGreen"];
            crystalCount ["MediumPurple"] += rewardsEL ["MediumPurple"];
            crystalCount ["MediumRed"] += rewardsEL ["MediumRed"];
            crystalCount ["MediumYellow"] += rewardsEL ["MediumYellow"];
            crystalCount ["LargeBlue"] += rewardsEL ["LargeBlue"];
            crystalCount ["LargeGreen"] += rewardsEL ["LargeGreen"];
            crystalCount ["LargePurple"] += rewardsEL ["LargePurple"];
            crystalCount ["LargeRed"] += rewardsEL ["LargeRed"];
            crystalCount ["LargeYellow"] += rewardsEL ["LargeYellow"];
            crystalCount ["HugeBlue"] += rewardsEL ["HugeBlue"];
            crystalCount ["HugeGreen"] += rewardsEL ["HugeGreen"];
            crystalCount ["HugePurple"] += rewardsEL ["HugePurple"];
            crystalCount ["HugeRed"] += rewardsEL ["HugeRed"];
            crystalCount ["HugeYellow"] += rewardsEL ["HugeYellow"];

            // Parsing the Crystal Dict<string, int> to a Dict<string, string> for Playfab Storage
            crystalParseDict1 ["SmallBlue"] = "" + crystalCount ["SmallBlue"];
            crystalParseDict1 ["SmallGreen"] = "" + crystalCount ["SmallGreen"];
            crystalParseDict1 ["SmallPurple"] = "" + crystalCount ["SmallPurple"];
            crystalParseDict1 ["SmallRed"] = "" + crystalCount ["SmallRed"];
            crystalParseDict1 ["SmallYellow"] = "" + crystalCount ["SmallYellow"];
            crystalParseDict1 ["MediumBlue"] = "" + crystalCount ["MediumBlue"];
            crystalParseDict1 ["MediumGreen"] = "" + crystalCount ["MediumGreen"];
            crystalParseDict1 ["MediumPurple"] = "" + crystalCount ["MediumPurple"];
            crystalParseDict1 ["MediumRed"] = "" + crystalCount ["MediumRed"];
            crystalParseDict1 ["MediumYellow"] = "" + crystalCount ["MediumYellow"];
            crystalParseDict2 ["LargeBlue"] = "" + crystalCount ["LargeBlue"];
            crystalParseDict2 ["LargeGreen"] = "" + crystalCount ["LargeGreen"];
            crystalParseDict2 ["LargePurple"] = "" + crystalCount ["LargePurple"];
            crystalParseDict2 ["LargeRed"] = "" + crystalCount ["LargeRed"];
            crystalParseDict2 ["LargeYellow"] = "" + crystalCount ["LargeYellow"];
            crystalParseDict2 ["HugeBlue"] = "" + crystalCount ["HugeBlue"];
            crystalParseDict2 ["HugeGreen"] = "" + crystalCount ["HugeGreen"];
            crystalParseDict2 ["HugePurple"] = "" + crystalCount ["HugePurple"];
            crystalParseDict2 ["HugeRed"] = "" + crystalCount ["HugeRed"];
            crystalParseDict2 ["HugeYellow"] = "" + crystalCount ["HugeYellow"];

            // Storing the new data to the server
            requestReturned = false;
            requestSuccess = false;
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = crystalParseDict1
            },
                CrystalStorageSuccess,
                OnErrorShared
            );
            // Wait for first request to return before sending the 2nd half. 15s max.
            while (!requestReturned)
            {
                yield return new WaitForSeconds(0.1f);
                secondCounter += 0.1f;
                if (secondCounter > 15.0f)
                {
                    break;
                    // TODO: Set notification that internet timed out.
                }
            }
            requestReturned = false;
            requestSuccess = false;
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = crystalParseDict2
            },
                CrystalStorageSuccess,
                OnErrorShared
            );
        }
        else
        {
            // TODO: Notification that couldn't connect.
        }
    }

    // May end up removing this and changing it to UpdateUserDataSuccess
    private void CrystalStorageSuccess(UpdateUserDataResult result)
    {
        requestReturned = true;
        requestSuccess = true;
    }
    #endregion CrystalHandlers

    // Caution: If changing languages, make sure new white space chars are accounted for string sanitiztaion.
    #region LevelData
    public void OnGetCompletedLevelData(string setLevelName)
    {
        requestReturned = false;
        requestSuccess = false;
        levelName = setLevelName.Replace(" ", string.Empty);
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = userID,
            Keys = new List<string>() {
                levelName
            }
        },
            UserDataLevelCallback,
            OnErrorShared);
    }
    
    public void OnUpdateLevelCompletionData(string setLevelName, string performance)
    {
        levelName = setLevelName.Replace(" ", string.Empty);
        requestReturned = false;
        requestSuccess = false;
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { levelName, performance } }
        },
        UpdateUserLevelDataSuccess,
        OnErrorShared);
    }

    public void OnGetLevelsCompleted()
    {
        requestReturned = false;
        requestSuccess = false;
        List<string> keys = new List<string>() { "CompletedLevels" };
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = userID,
            Keys = keys,
        },
        UserDataCompletedLevelsCallBack,
        OnErrorShared);
    }

    public void OnSetLevelsCompleted(int completed)
    {
        requestReturned = false;
        requestSuccess = false;
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "CompletedLevels", completed.ToString() } }
        },
        UpdateUserDataSuccess,
        OnErrorShared);
    }

    private void UpdateUserLevelDataSuccess(UpdateUserDataResult result)
    {
        requestReturned = true;
        requestSuccess = true;
    }

    // Sets the specific level's statistics if it was completed.
    private void UserDataLevelCallback(GetUserDataResult result)
    {
        if (result.Data.ContainsKey(levelName))
        {
            levelCompletionData = result.Data [levelName].Value;
        }
        else
            levelCompletionData = "";
        requestReturned = true;
        requestSuccess = true;
    }
    // Sets the total levels completed
    private void UserDataCompletedLevelsCallBack(GetUserDataResult result)
    {
        if (result.Data.ContainsKey("CompletedLevels"))
        {
            completedLevelCount = int.Parse(result.Data["CompletedLevels"].Value);
        }
        else
            completedLevelCount = 0;
        requestReturned = true;
        requestSuccess = true;
    }
    #endregion LevelData

    private void UpdateUserDataSuccess(UpdateUserDataResult result)
    {
        requestReturned = true;
        requestSuccess = true;
    }
    private void OnCloudUpdateScript(ExecuteCloudScriptResult result)
    {
        // Cloud Script returns arbitrary results, so you have to evaluate them one step and one parameter at a time
        Debug.Log("OnCloudUpdateScript: Fired appropriately");
        requestSuccess = true;
        requestReturned = true;
    }

    // This will trigger if there's no connection to the internet or an error in transmission.
    // TODO: Need to handle failures appropriately.
    private void OnErrorShared(PlayFabError error)
    {
        requestSuccess = false;
        requestReturned = true;
        Debug.Log(error.GenerateErrorReport());
    }
}