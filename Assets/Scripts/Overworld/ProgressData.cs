using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressData : MonoBehaviour
{
    // TODO: Get data from Server and populate
    // TODO: Spell/Staff levels
    // TODO: Player Level and data
    // TODO: Inventory and rewards acquired
    // TODO: Wisp Count
    // TODO: get list of strings of skins available;
    // TODO: When someone gets a new skin, update the playfab data and the data here immediately.
    
    private Dictionary<string, int> crystalInventory;
    private PlayFabData pfd;

    private void Awake()
    {
        crystalInventory = new Dictionary<string, int>()
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

        // ----------------------- Test Code -------------------------------
        
    }
    private void Start()
    {
        pfd = GameObject.Find("GlobalGameManager/PlayFabController/PlayFabDataController").GetComponent<PlayFabData>();
        PopulateCrystalInventory();
        
    }
    
    
    private void PopulateCrystalInventory()
    {
        pfd.GetUserCrystalData();
        StartCoroutine(GetPlayFabCrystals());
    }

    private IEnumerator GetPlayFabCrystals()
    {
        float secondCounter = 0;
        while (!pfd.GetRequestReturned())
        {
            yield return new WaitForSeconds(0.1f);
            secondCounter += 0.1f;
            if (secondCounter > 15.0f)
            {
                GlobalGameManager.instance.UseNotification("Connection to Server timed out.", 3.0f);
                break;
            }
        }
        if (pfd.GetRequestSuccess())
        {
            crystalInventory = pfd.GetCrystalCount();
        }
        else
            Debug.Log("Couldn't populate Crystal Inventory");
    }

    public Dictionary<string, int> GetCrystalInventory()
    {
        return crystalInventory;
    }
}
