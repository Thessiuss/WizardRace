using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OWSpinnerController : MonoBehaviour
{
    [Range(0, 2)]
    private int spinnerIndex;
    private GameObject spinnerPanel;
    private List<GameObject> spinner8Prefabs;
    private List<GameObject> spinner12Prefabs;
    private List<GameObject> spinner16Prefabs;
    private OWSpinnerData owsd;

    // TODO: Check if subscription, change the spinner
    // TODO: add tabs to each spinner.
    // TODO: Rare: 0-1%, Uncommon 1-35%, Common 35 - 100%
    private void Awake()
    {
        // TODO: Check which spinner is being used.
        // TODO: populate the "spins available."
        // TODO: Populate the rewards on the spinner according to a seed (which is current date)
    }

    private void Start()
    {
        owsd = GameObject.Find("Data/ClientData").GetComponent<OWSpinnerData>();
        spinnerPanel = GameObject.Find("UICanvas1/SpinnerPanel/SpinnerPanel");
        spinner16Prefabs = owsd.GetSpinner16();
        spinner12Prefabs = owsd.GetSpinner12();
        spinner8Prefabs = owsd.GetSpinner8();
        PopulateSpinner(0, spinner16Prefabs);
        PopulateSpinner(1, spinner12Prefabs);
        PopulateSpinner(2, spinner8Prefabs);
    }

    private void PopulateSpinner(int spinnerIndex, List<GameObject> spinnerPrefabs)
    {
        GameObject parent = spinnerPanel.transform.GetChild(4 + spinnerIndex).gameObject;
        // i have no idea how to make this a for loop since the parent.transform is specific.
        PSPrefabHandler(spinnerPrefabs[0], parent.transform.GetChild(0));
        PSPrefabHandler(spinnerPrefabs[1], parent.transform.GetChild(1));
        PSPrefabHandler(spinnerPrefabs[2], parent.transform.GetChild(spinnerPrefabs.Count - 1));
        PSPrefabHandler(spinnerPrefabs[3], parent.transform.GetChild(2));
        PSPrefabHandler(spinnerPrefabs[4], parent.transform.GetChild(3));
        PSPrefabHandler(spinnerPrefabs[5], parent.transform.GetChild(4));
        PSPrefabHandler(spinnerPrefabs[6], parent.transform.GetChild(spinnerPrefabs.Count - 2));
        PSPrefabHandler(spinnerPrefabs[7], parent.transform.GetChild(spinnerPrefabs.Count - 3));
        if (spinnerPrefabs.Count > 8)
        {
            for (int i = 0; i < spinnerPrefabs.Count - 8; i++)
            {
                PSPrefabHandler(spinnerPrefabs[i + 7], parent.transform.GetChild(5 + i));
            }
        }
    }

    private void PSPrefabHandler(GameObject go, Transform parent)
    {
        // Spawn the prefab data and then spawn the model.
        GameObject prefab = Instantiate(go, parent, false);
        Reward reward = prefab.GetComponent<Reward>();
        GameObject rewardModel = reward.GetPrefab();
        GameObject uiModel = Instantiate(rewardModel, prefab.transform, false);
        uiModel.transform.localScale = new Vector3(100, 100, 100);
    }

    public void OnActivateSpinner(int spinnerIndex)
    { 
        // TODO: pick out the reward based on a random number
        // TODO: spin the spinner 3x around
        // TODO: spin the spinner to the appropriate location given the reward.
    }
    
    public void OnSpinnerSelection(int index)
    {
        // TODO: change spinners via tabs on side of UI.
        // TODO: Give rewards to PlayFab and repopulate acquisition data.
    }
}
