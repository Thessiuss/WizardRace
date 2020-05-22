using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour, IUnityAdsListener
{

    private bool testMode = true;
#if UNITY_IOS
    private string gameId = "3482080";
#elif UNITY_ANDROID
    private string gameId = "3482081";
#endif

    private string[] rewardType = new string[]
    {
        "potion1",
        "potion2",
        "extraLife",
        "ancientChest"
    };
    private bool adReady = false;
    private string placementIdType = "rewardedVideo";

    private void Start()
    {
        adReady = Advertisement.IsReady(placementIdType);
        Advertisement.Initialize(gameId, testMode);
    }

    public void ShowRewardedVideo(int rewardType)
    {
        Advertisement.Show(placementIdType);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == placementIdType)
        {
            //mybutton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.Log("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
